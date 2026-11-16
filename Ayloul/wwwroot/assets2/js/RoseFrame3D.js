/**
 * RoseFrame3D
 * ---------------------------------------------------------------------------
 * A self-contained, reusable Three.js component that loads the rose-frame
 * .glb model into a <canvas>, lights it like a soft studio product shot, and
 * exposes a tiny public API so a page can drive its rotation from scroll
 * position and let the user spin it with mouse / touch.
 *
 * Usage:
 *   import { RoseFrame3D } from './RoseFrame3D.js';
 *   const viewer = new RoseFrame3D(canvasEl, { modelUrl: '/assets/models/rose_frame.glb' });
 *   await viewer.load();
 *   viewer.setScrollRotation(progress01);   // 0 -> 0deg, 1 -> 180deg (eased internally)
 *   viewer.start();  // begins the render loop
 *   viewer.stop();   // stops the render loop (call when off-screen)
 * ---------------------------------------------------------------------------
 */

import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { DRACOLoader } from 'three/addons/loaders/DRACOLoader.js';
import { RoomEnvironment } from 'three/addons/environments/RoomEnvironment.js';

const DEG2RAD = Math.PI / 180;

export class RoseFrame3D {
  /**
   * @param {HTMLCanvasElement} canvas
   * @param {Object} opts
   * @param {string} opts.modelUrl                path to rose_frame.glb
   * @param {number} [opts.maxRotationDeg=180]     total scroll-driven rotation
   * @param {number} [opts.scrollSmoothing=0.08]   0..1, higher = snappier
   * @param {number} [opts.dragDamping=0.92]       velocity decay per frame while coasting
   * @param {number} [opts.dragSensitivity=0.010]  radians of rotation per pixel dragged
   */
  constructor(canvas, opts = {}) {
    this.canvas = canvas;
    this.modelUrl = opts.modelUrl;
    this.maxRotation = (opts.maxRotationDeg ?? 180) * DEG2RAD;
    this.scrollSmoothing = opts.scrollSmoothing ?? 0.08;
    this.dragDamping = opts.dragDamping ?? 0.92;
    this.dragSensitivity = opts.dragSensitivity ?? 0.010;

    // rotation state -------------------------------------------------------
    this._scrollProgress = 0;      // raw 0..1 target set from outside
    this._scrollRotation = 0;      // smoothed rotation currently applied (radians)
    this._scrollRotationTarget = 0;
    this._dragOffset = 0;          // extra rotation from user dragging (radians)
    this._dragVelocity = 0;        // radians/frame, decays via dragDamping
    this._isDragging = false;
    this._lastPointerX = 0;
    this._activePointerId = null;

    this._running = false;
    this._rafId = null;
    this._ready = false;

    this._initScene();
    this._bindPointerEvents();
  }

  // ---------------------------------------------------------------------
  // Scene / lighting / camera setup
  // ---------------------------------------------------------------------
  _initScene() {
    const renderer = new THREE.WebGLRenderer({
      canvas: this.canvas,
      antialias: true,
      alpha: true,
      powerPreference: 'high-performance',
    });
    renderer.setPixelRatio(Math.min(window.devicePixelRatio || 1, 2));
    renderer.outputColorSpace = THREE.SRGBColorSpace;
    renderer.toneMapping = THREE.ACESFilmicToneMapping;
    renderer.toneMappingExposure = 1.05;
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    this.renderer = renderer;

    const scene = new THREE.Scene();
    scene.background = null; // let the page background show through
    this.scene = scene;

    // soft image-based fill light / subtle reflections
    const pmrem = new THREE.PMREMGenerator(renderer);
    const envTex = pmrem.fromScene(new RoomEnvironment(), 0.04).texture;
    scene.environment = envTex;
    pmrem.dispose();

    const camera = new THREE.PerspectiveCamera(32, 1, 0.05, 10);
    camera.position.set(0, 0.02, 0.95);
    camera.lookAt(0, 0, 0);
    this.camera = camera;

    // --- studio lighting rig ---
    const key = new THREE.DirectionalLight(0xfff6ec, 2.4);
    key.position.set(0.6, 0.9, 1.1);
    key.castShadow = true;
    key.shadow.mapSize.set(1024, 1024);
    key.shadow.camera.near = 0.2;
    key.shadow.camera.far = 3;
    key.shadow.camera.left = -0.4;
    key.shadow.camera.right = 0.4;
    key.shadow.camera.top = 0.4;
    key.shadow.camera.bottom = -0.4;
    key.shadow.bias = -0.0015;
    key.shadow.radius = 4;
    scene.add(key);

    const fill = new THREE.DirectionalLight(0xeaf1ff, 0.7);
    fill.position.set(-1, 0.3, 0.6);
    scene.add(fill);

    const rim = new THREE.DirectionalLight(0xffffff, 0.9);
    rim.position.set(-0.3, 0.6, -1.2);
    scene.add(rim);

    const hemi = new THREE.HemisphereLight(0xffffff, 0xe4ded6, 0.55);
    scene.add(hemi);

    // soft shadow-catcher plane beneath the frame
    const shadowMat = new THREE.ShadowMaterial({ opacity: 0.18 });
    const catcher = new THREE.Mesh(new THREE.PlaneGeometry(4, 4), shadowMat);
    catcher.rotation.x = -Math.PI / 2;
    catcher.position.y = -0.205;
    catcher.receiveShadow = true;
    scene.add(catcher);

    // pivot group: everything rotation-driven hangs off this
    this.pivot = new THREE.Group();
    scene.add(this.pivot);

    this._resize();
  }

  // ---------------------------------------------------------------------
  // Model loading
  // ---------------------------------------------------------------------
  async load() {
    const loader = new GLTFLoader();
    // Draco is optional here (the model isn't draco-compressed) but wiring it
    // up costs nothing and future-proofs the loader if you swap in a
    // compressed asset later.
    const draco = new DRACOLoader();
    draco.setDecoderPath('https://www.gstatic.com/draco/versioned/decoders/1.5.6/');
    loader.setDRACOLoader(draco);

    const gltf = await loader.loadAsync(this.modelUrl);
    const model = gltf.scene;

    model.traverse((obj) => {
      if (obj.isMesh) {
        obj.castShadow = true;
        obj.receiveShadow = true;
        if (obj.material) {
          obj.material.envMapIntensity = 0.6;
        }
      }
    });

    // Recenter defensively in case the export pivot ever changes.
    const box = new THREE.Box3().setFromObject(model);
    const center = box.getCenter(new THREE.Vector3());
    model.position.sub(center);

    this.pivot.add(model);
    this.model = model;
    this._ready = true;
    return model;
  }

  // ---------------------------------------------------------------------
  // Public API: scroll-driven rotation
  // ---------------------------------------------------------------------
  /** progress: 0 (top of section) .. 1 (bottom of section) */
  setScrollProgress(progress) {
    this._scrollProgress = Math.min(1, Math.max(0, progress));
    const eased = easeInOutCubic(this._scrollProgress);
    this._scrollRotationTarget = eased * this.maxRotation;
  }

  // ---------------------------------------------------------------------
  // Pointer / touch drag-to-rotate with inertia
  // ---------------------------------------------------------------------
  _bindPointerEvents() {
    const el = this.canvas;
    el.style.touchAction = 'pan-y'; // allow vertical page scroll, capture horizontal drag
    el.style.cursor = 'grab';

    const onPointerDown = (e) => {
      this._isDragging = true;
      this._activePointerId = e.pointerId;
      this._lastPointerX = e.clientX;
      this._dragVelocity = 0;
      el.setPointerCapture(e.pointerId);
      el.style.cursor = 'grabbing';
    };
    const onPointerMove = (e) => {
      if (!this._isDragging || e.pointerId !== this._activePointerId) return;
      const dx = e.clientX - this._lastPointerX;
      this._lastPointerX = e.clientX;
      const delta = dx * this.dragSensitivity;
      this._dragOffset += delta;
      this._dragVelocity = delta; // used for post-release coasting
    };
    const endDrag = (e) => {
      if (e && e.pointerId !== this._activePointerId) return;
      this._isDragging = false;
      this._activePointerId = null;
      el.style.cursor = 'grab';
    };

    el.addEventListener('pointerdown', onPointerDown);
    el.addEventListener('pointermove', onPointerMove);
    el.addEventListener('pointerup', endDrag);
    el.addEventListener('pointercancel', endDrag);
    el.addEventListener('pointerleave', endDrag);

    this._disposePointerEvents = () => {
      el.removeEventListener('pointerdown', onPointerDown);
      el.removeEventListener('pointermove', onPointerMove);
      el.removeEventListener('pointerup', endDrag);
      el.removeEventListener('pointercancel', endDrag);
      el.removeEventListener('pointerleave', endDrag);
    };
  }

  // ---------------------------------------------------------------------
  // Render loop
  // ---------------------------------------------------------------------
  start() {
    if (this._running) return;
    this._running = true;
    const tick = () => {
      if (!this._running) return;
      this._update();
      this.renderer.render(this.scene, this.camera);
      this._rafId = requestAnimationFrame(tick);
    };
    this._rafId = requestAnimationFrame(tick);
  }

  stop() {
    this._running = false;
    if (this._rafId) cancelAnimationFrame(this._rafId);
    this._rafId = null;
  }

  _update() {
    // smooth (cinematic) approach toward the scroll-derived target
    this._scrollRotation += (this._scrollRotationTarget - this._scrollRotation) * this.scrollSmoothing;

    // inertial coasting of the manual drag once released
    if (!this._isDragging) {
      if (Math.abs(this._dragVelocity) > 0.0001) {
        this._dragOffset += this._dragVelocity;
        this._dragVelocity *= this.dragDamping;
      } else {
        this._dragVelocity = 0;
      }
    }

    if (this.pivot) {
      this.pivot.rotation.y = this._scrollRotation + this._dragOffset;
    }
  }

  // ---------------------------------------------------------------------
  // Responsive sizing
  // ---------------------------------------------------------------------
  resize() { this._resize(); }

  _resize() {
    const parent = this.canvas.parentElement;
    const w = parent ? parent.clientWidth : window.innerWidth;
    const h = parent ? parent.clientHeight : window.innerHeight;
    this.renderer.setSize(w, h, false);
    this.camera.aspect = w / h;
    this.camera.updateProjectionMatrix();
  }

  // ---------------------------------------------------------------------
  dispose() {
    this.stop();
    this._disposePointerEvents?.();
    this.scene?.traverse((obj) => {
      if (obj.isMesh) {
        obj.geometry?.dispose();
        if (Array.isArray(obj.material)) obj.material.forEach((m) => m.dispose());
        else obj.material?.dispose();
      }
    });
    this.renderer?.dispose();
  }
}

function easeInOutCubic(t) {
  return t < 0.5 ? 4 * t * t * t : 1 - Math.pow(-2 * t + 2, 3) / 2;
}
