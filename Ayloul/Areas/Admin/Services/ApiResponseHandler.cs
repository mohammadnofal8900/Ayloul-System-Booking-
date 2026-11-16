using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ayloul.Areas.Admin.Services
{
    public class ApiResponseHandler
    {
        public IActionResult? Handle(
            HttpResponseMessage response,
            Controller controller)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return controller.RedirectToAction(
                        "Login",
                        "Account",
                        new { area = "User" }
                    );

                case HttpStatusCode.Forbidden:
                    return controller.RedirectToAction(
                        "Error",
                        "Error",
                        new { area = "" }
                    );

                case HttpStatusCode.NotFound:
                    return controller.NotFound();

                case HttpStatusCode.BadRequest:
                    return controller.BadRequest();

                case HttpStatusCode.Conflict:
                    return controller.Conflict();

                case HttpStatusCode.UnprocessableEntity:
                    return controller.UnprocessableEntity();

                case HttpStatusCode.InternalServerError:
                    return controller.View("Error");

                case HttpStatusCode.ServiceUnavailable:
                    return controller.View("Error");

                case HttpStatusCode.GatewayTimeout:
                    return controller.View("Error");

                default:
                    return null;
            }
        }
    }
}