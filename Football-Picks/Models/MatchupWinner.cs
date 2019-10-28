using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Picks.Models
{
    public class MatchupWinner
    {
        public string WinnerTeam { get; set; }
        public string PlayerPick { get; set; }
        public string WinTeamLogoUrl { get; set; }
        public string PlayerPickLogoUrl { get; set; }
        public string WinRecord { get; set; }
        public string PlayerPickRecord { get; set; }
        public string Win { get; set; }

        public MatchupWinner(string winnerTeam, string playerPick, string winTeamLogoUrl, string playerPickLogoUrl, string win)
        {
            WinnerTeam = winnerTeam;
            PlayerPick = playerPick;
            WinTeamLogoUrl = winTeamLogoUrl;
            PlayerPickLogoUrl = playerPickLogoUrl;
            Win = win;
        }

        public MatchupWinner(string winnerTeam, string playerPick, string winTeamLogoUrl, string playerPickLogoUrl, string winRecord, string playerPickRecord, string win)
        {
            WinnerTeam = winnerTeam;
            PlayerPick = playerPick;
            WinTeamLogoUrl = winTeamLogoUrl;
            PlayerPickLogoUrl = playerPickLogoUrl;
            WinRecord = winRecord;
            PlayerPickRecord = playerPickRecord;
            Win = win;
        }
    }
}
