using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Football_Picks.Data;
using Microsoft.AspNetCore.Mvc;

namespace Football_Picks.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
        }
    }
}