using System;
using Microsoft.AspNetCore.Mvc;

namespace Soonish.Forms.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return Redirect("swagger");
        }
    }
}
