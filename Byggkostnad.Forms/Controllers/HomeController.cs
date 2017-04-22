using System;
using Microsoft.AspNetCore.Mvc;

namespace Byggkostnad.Forms.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return Redirect("swagger");
        }
    }
}
