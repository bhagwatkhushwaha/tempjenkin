using Microsoft.AspNetCore.Mvc;
using Autumn.Web.Controllers;

namespace Autumn.Web.Public.Controllers
{
    public class HomeController : AutumnControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}