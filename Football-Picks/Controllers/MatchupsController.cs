using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Football_Picks.Data;
using Football_Picks.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Football_Picks.Controllers
{
    public class MatchupsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment env;
        private string week;

        //Constructor
        public MatchupsController(ApplicationDbContext db, IHostingEnvironment env)
        {
            this.db = db;
            this.env = env;
            week = MatchupDataHelper.Get_Current_Week();
        }

        //Get : Matchups
        [Authorize]
        public IActionResult Index()
        {
            var matchups = MatchupDataHelper.Load_Matchups(week);
            ViewBag.week = week;
            return View(matchups);
        }

        //Get : Matchup/week
        [HttpPost]
        [Authorize]
        public IActionResult Index(string _week)
        {
            var matchups = MatchupDataHelper.Load_Matchups(_week);
            ViewBag.week = _week;
            return View(matchups);
        }
    }
}