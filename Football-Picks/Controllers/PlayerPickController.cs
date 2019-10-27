using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Football_Picks.Data;
using Football_Picks.Helpers;
using Football_Picks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Football_Picks.Controllers
{
    public class PlayerPickController : Controller
    {
        //private Application Database Context
        private readonly ApplicationDbContext context;
        private string week;

        //Constructor
        public PlayerPickController(ApplicationDbContext context)
        {
            this.context = context;
            this.week = MatchupDataHelper.Get_Current_Week();
        }

        //GET : PlayerPicks/Picks
        [Authorize]
        public IActionResult Picks(int? id)
        {
           
            var picks = context.Pick.Where(p => p.PlayerId == id && p.Week == week).ToList();

            if (picks.Count() > 0)
            {
                var player = context.Players.FirstOrDefault(m => m.PlayerId == id);
                //ViewBag.status = "Not Found";
                var tieBreaker = picks[0].TieBreaker;
                List<string> teamAbr = new List<string>();

                foreach (var pick in picks)
                {
                    string abr = MatchupDataHelper.GetTeamAbr(pick.PlayerPick);
                    teamAbr.Add("/img/nfl-logo/" + abr + ".png");
                }
                
                ViewBag.player = player;
                ViewBag.picks = picks;
                ViewBag.abr = teamAbr;
                ViewBag.tieBreaker = tieBreaker;

                return View("Details", player);
            }
            else
            {
                var matchups = MatchupDataHelper.Load_Matchups(week);
                var player = context.Players.FirstOrDefault(m => m.PlayerId == id);
                ViewBag.playerId = id;
                ViewBag.player = player;
                return View(matchups);
            }
           
        }

        [Authorize]
        public IActionResult Details(int? id)
        {
            var player = context.Players.Where(p => p.PlayerId == id).FirstOrDefault();
            var picks = context.Pick.Where(p => p.PlayerId == id && p.Week == week).ToList();
            var tieBreaker = picks[0].TieBreaker;

            List<string> teamAbr = new List<string>();

            if (picks.Count > 0)
            {
                foreach (var pick in player.Picks)
                {
                    string abr = MatchupDataHelper.GetTeamAbr(pick.PlayerPick);
                    teamAbr.Add("/img/nfl-logo/" + abr + ".png");
                }
            }

            ViewBag.picks = picks;
            ViewBag.abr = teamAbr;
            ViewBag.tieBreaker = tieBreaker;
            return View(player);
        }

        //GET : PlayerPicks
        //Load all the Picks for the player that matches the id passed in from the View.
        [Authorize]
        public IActionResult Index(int? id) 
        {
            var picks = context.Pick.Where(p => p.PlayerId == id && p.Week == week).ToList();
            return View(picks);
        }

        //Save Picks
        [HttpPost]
        [Authorize]
        public IActionResult SavePicks(int? id, string[] teams, string tiebreaker)
        {
            string today_Date = DateTime.Now.ToString("M/d/yyy");
            var matchups = MatchupDataHelper.Load_Matchups(week);
            int matchupCount = matchups.Count();
            
            if (matchupCount != teams.Length || tiebreaker == null)
            {
                return View(nameof(NotSaved));
            }

            var picks = context.Pick.Where(p => p.PlayerId == id && p.Week == week);
            int _tie;

            bool tieSucess = int.TryParse(tiebreaker, out _tie);
            bool status = false;

            if (picks.Count() > 0)
            {       
                return RedirectToAction(nameof(AlreadyPicked));
            }
            else
            {
                if (id != null && teams.Length > 0)
                {
                    var player = context.Players.Find(id);
                    foreach (var team in teams)
                    {
                        Pick pick = new Pick(player.PlayerId, team, _tie, week, "2019");
                        context.Add(pick);
                        context.SaveChanges();
                        status = true;
                    }
                    
                    if (status)
                    {
                        ViewBag.player = player;
                        return RedirectToAction(nameof(PicksSaved));
                    }
                    else
                        return RedirectToAction(nameof(NotSaved));

                }
                else
                {
                    return RedirectToAction(nameof(NotSaved));
                }
            }
           
        }

        [Authorize]
        public IActionResult CalculateWins(int? id)
        {
            var picks = context.Pick.Where(p => p.PlayerId == id && p.Week == week).ToList();
            var player = context.Players.Where(p => p.PlayerId == id).Select(p => p.Name).SingleOrDefault();
            List<MatchupWinner> wins = new List<MatchupWinner>();
            int winCount = 0;

            if (picks.Count() > 1)
            {
                wins = MatchupDataHelper.CalculateWins(picks);
                foreach (var win in wins)
                {
                    if (win.Win.Equals("YES"))
                    {
                        winCount++;
                    }
                }
                ViewBag.tiebreaker = picks[0].TieBreaker;
                ViewBag.player = player;
                ViewBag.wincount = winCount;
                return View(wins);
            }
            else
            {
                ViewBag.status = "Not Found";
                return View(nameof(CalculateWins));
            }
            
        }

        public IActionResult PicksSaved()
        {
            return View();
        }

        public IActionResult NotSaved()
        {
            return View();
        }

        public IActionResult AlreadyPicked()
        {
            return View();
        }
    }
}