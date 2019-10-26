using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Picks.Models
{
    public class PlayerWins
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public int WinCount { get; set; }

        public PlayerWins(string name, string company, int winCount)
        {
            Name = name;
            Company = company;
            WinCount = winCount;
        }
    }
}
