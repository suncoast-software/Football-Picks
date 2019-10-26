using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Picks.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Team_Name { get; set; }
        public string Team_Abr { get; set; }
        public string Team_Record { get; set; }
        public string Score { get; set; }
        public string Logo_Url { get; set; }

        public Team()
        {
        }

        public Team(string team_Name, string team_Abr, string team_Record, string score)
        {
            Team_Name = team_Name;
            Team_Abr = team_Abr;
            Team_Record = team_Record;
            Score = score;
        }

        public Team(string team_Name, string team_Abr, string team_Record, string score, string logo_Url)
        {
            Team_Name = team_Name;
            Team_Abr = team_Abr;
            Team_Record = team_Record;
            Score = score;
            Logo_Url = logo_Url;
        }
    }
}
