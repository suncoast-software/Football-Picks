using System;
using System.Collections.Generic;
using System.Text;
using Football_Picks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Football_Picks.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Pick> Pick { get; set; }
        public DbSet<Matchup> Matchups { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
    }
}
