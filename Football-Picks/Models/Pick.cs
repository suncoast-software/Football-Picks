using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Picks.Models
{
    public class Pick
    {
        [Key]
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public string PlayerPick { get; set; }
        public int TieBreaker { get; set; }
        public string Week { get; set; }
        public string Year { get; set; }

        public Pick()
        {
        }

        public Pick(int playerId, string pick, int tieBreaker, string week, string year)
        {
            PlayerId = playerId;
            PlayerPick = pick;
            TieBreaker = tieBreaker;
            Week = week;
            Year = year;
        }
    }
}
