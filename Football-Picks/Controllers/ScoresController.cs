using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Football_Picks.Data;
using Football_Picks.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Football_Picks.Controllers
{
    public class ScoresController : Controller
    {
        private readonly string week;
        private readonly ApplicationDbContext context;

        public ScoresController(ApplicationDbContext context)
        {
            this.context = context;
            week = MatchupDataHelper.Get_Current_Week();
        }

        [Authorize]
        public IActionResult Scores()
        {
            var scores = MatchupDataHelper.Get_Week_Scores(week);
            ViewBag.week = week;
            return View(scores);
        }
    }
}