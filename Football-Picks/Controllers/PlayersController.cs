using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Football_Picks.Data;
using Football_Picks.Models;
using Football_Picks.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Football_Picks.Controllers
{
    public class PlayersController : Controller
    {
        //private database context field
        private readonly ApplicationDbContext _context;
        private string week;

        //constructor
        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
            week = MatchupDataHelper.Get_Current_Week();
        }

        // GET: Players
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Players.ToListAsync());
        }

        // GET: Players/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FirstOrDefaultAsync(m => m.PlayerId == id);
            var picks = _context.Pick.Where(p => p.PlayerId == id && p.Week == week).ToList();
            var tieBreaker = 0;
            List<string> teamAbr = new List<string>();

            if (picks.Count > 0)
            {
                tieBreaker = picks[0].TieBreaker;
                foreach (var pick in player.Picks)
                {
                    string abr = MatchupDataHelper.GetTeamAbr(pick.PlayerPick);          
                    teamAbr.Add("/img/nfl-logo/" + abr + ".png");
                }
            }
           
            
            if (player == null)
            {
                return NotFound();
            }
            ViewBag.picks = picks;
            ViewBag.abr = teamAbr;
            ViewBag.tieBreaker = tieBreaker;
          
            return View(player);
        }

        // GET: Players/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PlayerId,Name,Company")] Player player)
        {
            var curPlayers = _context.Players;
            foreach (var p in curPlayers)
            {
                if (p.Name.Equals(player.Name) && p.Company.Equals(player.Company))
                {
                    return RedirectToAction(nameof(AlreadyCreated));
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Players/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PlayerId,Name,Company")] Player player)
        {
            if (id != player.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.PlayerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Players/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        //Calculate All Player Wins
        public IActionResult CalculateAllPlayerWins()
        {
            var players = _context.Players.ToList();
            List<PlayerWins> playerWinsList = new List<PlayerWins>();
            List<PlayerWins> newPlayerWinList = new List<PlayerWins>();
            List<PlayerWins> tieList = new List<PlayerWins>();

            int totalPoints = MatchupDataHelper.Get_Tie_Breaker_Points();

            foreach (var player in players)
            {
                int winCount = 0;
                var playerPicks = _context.Pick.Where(p => p.PlayerId == player.PlayerId && p.Week == week).ToList();
                var winList = MatchupDataHelper.CalculateWins(playerPicks).ToList();
                foreach (var item in winList)
                {
                    if (item.Win.Equals("YES"))
                    {
                        winCount++;
                    }
                }
                playerWinsList.Add(new PlayerWins(player.Name, player.Company, winCount));
                newPlayerWinList = playerWinsList.OrderByDescending(p => p.WinCount)
                                                   .ThenBy(p => p.Name)
                                                   .ToList();
            }
            
            return View(newPlayerWinList);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AlreadyCreated()
        {
            return View();
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
