// ---------------------------------------------------------------------
// Rose Frame — 2D scroll-driven animation.
//
// This replaces the previous Three.js/GLB implementation (RoseFrame3D).
// It intentionally reuses the exact same scroll-progress architecture as
// before (a single requestAnimationFrame loop reading
// section.getBoundingClientRect() — no `scroll` event listener, no
// second/competing scroll system) and simply drives CSS transforms on
// an <img> instead of a Three.js pivot rotation.
// ---------------------------------------------------------------------

// Single place to change the frame image if the path/format ever changes.
const FRAME_IMAGE_URL = "/images/Home-Page/Frame.png";

const section = document.querySelector('[data-rose-section]');
const stage = document.querySelector('[data-rose-stage]');
const floatEl = document.querySelector('[data-frame-float]');
const frameImg = document.querySelector('[data-frame-img]');

frameImg.src = FRAME_IMAGE_URL;

// Idle-float is on by default (paused via CSS until markScrolling() below
// starts toggling it based on real scroll activity).
floatEl.classList.add('is-idle');

// ---------------------------------------------------------------------
// Tunable animation values — desktop and a reduced mobile variant.
// ---------------------------------------------------------------------
const ANIM = {
  translateY: -60,   // px, reached at scroll progress === 1
  rotate: 5,         // deg
  scale: 1.06,
  smoothing: 0.08    // 0..1, higher = snappier (matches the old scrollSmoothing)
};

const ANIM_MOBILE = {
  translateY: -24,
  rotate: 2,
  scale: 1.03
};

const MOBILE_BREAKPOINT = 640;
const isMobile = () => window.innerWidth <= MOBILE_BREAKPOINT;
const currentAnim = () => (isMobile() ? ANIM_MOBILE : ANIM);

let sectionVisible = false;
let scrollTarget = 0;   // eased 0..1 target derived from scroll position
let scrollCurrent = 0;  // smoothed 0..1 value actually rendered

let mouseTargetX = 0;
let mouseTargetY = 0;
let mouseCurrentX = 0;
let mouseCurrentY = 0;

let lastProgress = -1;
let idleTimer = null;

// ---------------------------------------------------------------------
// Visibility observer — pause work while the section is off-screen.
// ---------------------------------------------------------------------
const visibilityObserver = new IntersectionObserver(
  (entries) => {
    for (const entry of entries) sectionVisible = entry.isIntersecting;
  },
  { threshold: 0 }
);
visibilityObserver.observe(section);

// ---------------------------------------------------------------------
// Scroll -> progress (identical math to the previous 3D implementation).
// ---------------------------------------------------------------------
function computeScrollProgress() {
  const rect = section.getBoundingClientRect();
  const vh = window.innerHeight;
  const scrollable = rect.height - vh;

  if (scrollable <= 0) {
    return rect.top <= 0 ? 1 : 0;
  }

  const scrolled = -rect.top;
  return Math.min(1, Math.max(0, scrolled / scrollable));
}

function easeInOutCubic(t) {
  return t < 0.5 ? 4 * t * t * t : 1 - Math.pow(-2 * t + 2, 3) / 2;
}

// ---------------------------------------------------------------------
// Idle float toggling — only runs while the user isn't actively
// scrolling, so it never fights the scroll-driven transform.
// ---------------------------------------------------------------------
function markScrolling() {
  floatEl.classList.remove('is-idle');
  clearTimeout(idleTimer);
  idleTimer = setTimeout(() => floatEl.classList.add('is-idle'), 160);
}

// ---------------------------------------------------------------------
// Subtle mouse parallax — desktop / fine-pointer only, disabled on
// touch devices, and always additive on top of (never conflicting
// with) the scroll-driven transform.
// ---------------------------------------------------------------------
const supportsHoverFine =
  window.matchMedia('(hover: hover) and (pointer: fine)').matches;

if (supportsHoverFine) {
  window.addEventListener('mousemove', (e) => {
    if (!sectionVisible) return;
    const rect = stage.getBoundingClientRect();
    const nx = ((e.clientX - rect.left) / rect.width) * 2 - 1;  // -1..1
    const ny = ((e.clientY - rect.top) / rect.height) * 2 - 1;  // -1..1
    mouseTargetX = Math.max(-1, Math.min(1, nx)) * 6; // px, very subtle
    mouseTargetY = Math.max(-1, Math.min(1, ny)) * 4; // px, very subtle
  });
}

// ---------------------------------------------------------------------
// Animation loop.
// ---------------------------------------------------------------------
function frameLoop() {
  const progress = computeScrollProgress();

  if (lastProgress !== -1 && progress !== lastProgress) {
    markScrolling();
  }
  lastProgress = progress;

  scrollTarget = easeInOutCubic(progress);
  scrollCurrent += (scrollTarget - scrollCurrent) * ANIM.smoothing;

  mouseCurrentX += (mouseTargetX - mouseCurrentX) * 0.1;
  mouseCurrentY += (mouseTargetY - mouseCurrentY) * 0.1;

  const v = currentAnim();
  const ty = scrollCurrent * v.translateY + mouseCurrentY;
  const tx = mouseCurrentX;
  const rot = scrollCurrent * v.rotate;
  const scale = 1 + scrollCurrent * (v.scale - 1);

  frameImg.style.transform =
    `translate3d(${tx.toFixed(2)}px, ${ty.toFixed(2)}px, 0) rotate(${rot.toFixed(2)}deg) scale(${scale.toFixed(4)})`;

  requestAnimationFrame(frameLoop);
}

requestAnimationFrame(frameLoop);
