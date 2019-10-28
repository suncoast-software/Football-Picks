using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Football_Picks.Models;
using Football_Picks.Helpers;

namespace Football_Picks.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string week = MatchupDataHelper.Get_Current_Week();
            //string record = MatchupDataHelper.Get_Team_Record("Ravens");
            ViewBag.week = week;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
