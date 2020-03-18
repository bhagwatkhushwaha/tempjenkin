using Microsoft.AspNetCore.Mvc;
using Autumn.Web.Controllers;

namespace Autumn.Web.Public.Controllers
{
    public class AboutController : AutumnControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}