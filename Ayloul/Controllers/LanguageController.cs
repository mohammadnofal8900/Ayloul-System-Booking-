using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Ayloul.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change_Language(string return_url,string culter)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culter)),
                new CookieOptions { Expires=DateTime.UtcNow.AddYears(1)}
                );
            return LocalRedirect( return_url ?? "/");
        }
    }
}
