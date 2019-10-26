using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Picks.Models
{
    public class Matchup
    {
        [Key]
        public int Id { get; set; }

        public Team AwayTeam { get; set; }
        public Team HomeTeam { get; set; }
        public string Week { get; set; }
        public string Year { get; set; }

        public Matchup()
        {
        }

        public Matchup(Team awayTeam, Team homeTeam, string week, string year)
        {
            AwayTeam = awayTeam;
            HomeTeam = homeTeam;
            Week = week;
            Year = year;
        }
    }
}
