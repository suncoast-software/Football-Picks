using Football_Picks.Data;
using Football_Picks.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Football_Picks.Helpers
{
    public class MatchupDataHelper
    {
        /// <summary>
        /// private field to hold the Application DB context for the database
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="context"></param>
        public MatchupDataHelper(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Calculate Player Wins
        /// </summary>
        /// <param name="Picks"></param>
        /// <returns>IEnumerable Pick</returns>
        #region CALCULATE PLYAER WINS
        public static List<MatchupWinner> CalculateWins(List<Pick> picks)
        {
            List<Team> winners = Get_Current_Week_Winners();
            List<MatchupWinner> winList = new List<MatchupWinner>();

            int index = 0;

            foreach (var winner in winners)
            {
                if (picks.Count > 0)
                {
                    
                   
                    //string pPick = pick.PlayerPick.ToString();
                    if (winners[index].Team_Name.Equals(picks[index].PlayerPick))
                    {
                        string playerPickAbr = GetTeamAbr(picks[index].PlayerPick);
                        string winAbr = GetTeamAbr(winners[index].Team_Name);
                        string winningTeamRecord  = Get_Team_Record(winner.Team_Name);
                        string curPickRecord = Get_Team_Record(picks[index].PlayerPick);
                        string pPickUrl = "/img/nfl-logo/" + playerPickAbr + ".png";
                        string winUrl = "/img/nfl-logo/" + winAbr + ".png";

                        winList.Add(new MatchupWinner(winners[index].Team_Name, picks[index].PlayerPick, winUrl, pPickUrl, winningTeamRecord, curPickRecord, "YES"));
                       

                    }
                    else
                    {
                        string playerPickAbr = GetTeamAbr(picks[index].PlayerPick);
                        string winAbr = GetTeamAbr(winners[index].Team_Name);
                        string winningTeamRecord = Get_Team_Record(winner.Team_Name);
                        string curPickRecord = Get_Team_Record(picks[index].PlayerPick);
                        string pPickUrl = "/img/nfl-logo/" + playerPickAbr + ".png";
                        string winUrl = "/img/nfl-logo/" + winAbr + ".png";

                        winList.Add(new MatchupWinner(winners[index].Team_Name, picks[index].PlayerPick, winUrl, pPickUrl, winningTeamRecord, curPickRecord, "NO"));
                       
                    }
                    index++;
                }
                else
                    return winList;
            }
            return winList;
        }
        #endregion

        /// <summary>
        /// Calculate Player Wins
        /// </summary>
        /// <param name="Picks"></param>
        /// <returns>IEnumerable Pick</returns>
        #region CALCULATE All PLYAER WINS
        public static List<MatchupWinner> CalculateAllWins(List<Pick> picks)
        {
            List<Team> winners = Get_Current_Week_Winners();
            List<MatchupWinner> winList = new List<MatchupWinner>();

            int index = 0;

            foreach (var winner in winners)
            {
                if (picks.Count > 0)
                {

                    
                    //string pPick = pick.PlayerPick.ToString();
                    if (winners[index].Team_Name.Equals(picks[index].PlayerPick))
                    {
                        string playerPickAbr = GetTeamAbr(picks[index].PlayerPick);
                        string winAbr = GetTeamAbr(winners[index].Team_Name);
                       // string winningTeamRecord = Get_Team_Record(winner.Team_Name);
                       // string curPickRecord = Get_Team_Record(picks[index].PlayerPick);
                        string pPickUrl = "/img/nfl-logo/" + playerPickAbr + ".png";
                        string winUrl = "/img/nfl-logo/" + winAbr + ".png";

                        winList.Add(new MatchupWinner(winners[index].Team_Name, picks[index].PlayerPick, winUrl, pPickUrl, "YES"));
                    }
                    else
                    {
                        string playerPickAbr = GetTeamAbr(picks[index].PlayerPick);
                        string winAbr = GetTeamAbr(winners[index].Team_Name);
                       //string winningTeamRecord = Get_Team_Record(winner.Team_Name);
                        //string curPickRecord = Get_Team_Record(picks[index].PlayerPick);
                        string pPickUrl = "/img/nfl-logo/" + playerPickAbr + ".png";
                        string winUrl = "/img/nfl-logo/" + winAbr + ".png";

                        winList.Add(new MatchupWinner(winners[index].Team_Name, picks[index].PlayerPick, winUrl, pPickUrl, "NO"));  
                    }
                    index++;
                }
                else
                    return winList;
            }
            return winList;
        }
        #endregion

        /// <summary>
        /// Get current week winners
        /// </summary>
        /// <returns>IEnumerable</returns>
        #region GET CURRENT WEEK WINNERS

        private static List<Team> Get_Current_Week_Winners()
        {
            List<Team> winners = new List<Team>();
            string url = "https://www.footballdb.com/scores/index.html";

            HtmlWeb page = new HtmlWeb();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table");

            foreach (var node in gameNodes)
            {
                if (node.ChildNodes[3].ChildNodes[1].ChildNodes.Count > 4)
                {
                    string awayTeam = node.ChildNodes[3].ChildNodes[1].ChildNodes[1].InnerText;
                    string homeTeam = node.ChildNodes[3].ChildNodes[3].ChildNodes[1].InnerText;
                    string awayScore = node.ChildNodes[3].ChildNodes[1].ChildNodes[7].InnerText;
                    string homeScore = node.ChildNodes[3].ChildNodes[3].ChildNodes[7].InnerText;

                    int recordIndex = awayTeam.IndexOf('(');
                    string awayRecord = awayTeam.Substring(recordIndex);
                    string awayTeamName = awayTeam.Replace(awayRecord, "").Trim();
                    recordIndex = homeTeam.IndexOf('(');
                    string homeRecord = homeTeam.Substring(recordIndex);
                    string homeTeamName = homeTeam.Replace(homeRecord, "").Trim();

                    int aScore;
                    int hScore;

                    bool awayScoreResult = int.TryParse(awayScore, out aScore);
                    bool homeScoreResult = int.TryParse(homeScore, out hScore);

                    if (awayScoreResult && homeScoreResult)
                    {
                        if (aScore > hScore)
                        {
                            string abr = GetTeamAbr(awayTeamName);
                            string logoUrl = @"\img\nff-logo\" + abr + ".png";
                            winners.Add(new Team(awayTeamName, abr, awayRecord, awayScore, logoUrl));
                         
                        }
                        else
                        {
                            string abr = GetTeamAbr(homeTeamName);
                            string logoUrl = @"\img\nff-logo\" + abr + ".png";
                            winners.Add(new Team(homeTeamName, abr, homeRecord, homeScore, logoUrl));
                        }
                        
                    }
                }
                
            }

            return winners;
        }

        #endregion

        /// <summary>
        /// Calculate the Tie Breaker winning Team
        /// </summary>
        /// <param name="away"></param>
        /// <param name="home"></param>
        /// <returns>Team</returns>
        #region CALCULATE TIE BREAKER

        public static int Get_Tie_Breaker_Points()
        {
            //calculate winning team
            int totalPoints = 0;
            string url = "https://www.footballdb.com/scores/index.html";
            HtmlWeb page = new HtmlWeb();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection scoreNodes = doc.DocumentNode.SelectNodes("//table");

            HtmlNode teamNodes = scoreNodes[scoreNodes.Count - 1];
            bool awayResult = int.TryParse(teamNodes.ChildNodes[3].ChildNodes[1].ChildNodes[3].InnerText, out int awayPoints);
            bool homeResult = int.TryParse(teamNodes.ChildNodes[3].ChildNodes[3].ChildNodes[3].InnerText, out int homePoints);

            if (awayResult == false || homeResult == false)
            {
                return 0;
            }
            else
            {
                totalPoints = awayPoints + homePoints;
            }

            return totalPoints;
        }

        #endregion

        /// <summary>
        /// load selected week matchups
        /// </summary>
        /// <param name="week"></param>
        /// <returns>List</returns>
        #region LOAD MATCHUPS
        public static List<Matchup> Load_Matchups(string week)
        {
            string url = "https://www.footballdb.com/scores/index.html?lg=NFL&yr=2019&type=reg&wk=" + week;

            List<Matchup> matchups = new List<Matchup>();
            HtmlWeb page = new HtmlWeb();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table//tbody//tr");

            for (int i = 0; i < gameNodes.Count - 1; i += 2)
            {
                //AwayTeam
                string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split('(');
                string awayRecord = awayDetails[1].Replace(")", "");
                string awayTeamName = awayDetails[0].Trim();
                // string awayFinalScore = gameNodes[i].ChildNodes[7].InnerText;
                string awayAbr = GetTeamAbr(awayTeamName.Trim());
                string awayLogoUrl = "/img/nfl-logo/" + awayAbr + ".png";

                //HomeTeam
                string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                string homeRecord = homeDetails[1].Replace(")", "");
                string homeTeamName = homeDetails[0].Trim();
                //string homeFinalScore = gameNodes[i + 1].ChildNodes[7].InnerText;
                string homeAbr = GetTeamAbr(homeTeamName.Trim());
                string homeLogoUrl = "/img/nfl-logo/" + homeAbr + ".png";

                Matchup matchup = new Matchup(new Team(awayTeamName, awayAbr, awayRecord, "", awayLogoUrl),
                    new Team(homeTeamName, homeAbr, homeRecord, "", homeLogoUrl), week, "2019");

                matchups.Add(matchup);
            }

            return matchups;
        }
        #endregion

        /// <summary>
        /// Get the current week 
        /// </summary>
        /// <returns>string</returns>
        #region GET CURRENT WEEK
        public static string Get_Current_Week()
        {
            string pageUrl = "https://www.footballdb.com/scores/index.html";

            HtmlWeb page = new HtmlWeb();
            HtmlDocument doc = page.Load(pageUrl);

            string[] weekDetails = doc.DocumentNode.SelectSingleNode("//div[@id='leftcol']//h1").InnerText.Split('-');
            string week = weekDetails[1].Replace("Week", "").Trim();

            return week;
        }
        #endregion

        /// <summary>
        /// Get the selected week scores
        /// </summary>
        /// <param name="week"></param>
        /// <returns>List</returns>
        #region GET WEEK SCORES
        public static List<Matchup> Get_Week_Scores(string week)
        {
            List<Matchup> scores = new List<Matchup>();
            string url = "https://www.footballdb.com/scores/index.html?lg=NFL&yr=2019&type=reg&wk=" + week;

            HtmlWeb page = new HtmlWeb();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table//tbody//tr");

            for (int i = 0; i < gameNodes.Count - 1; i += 2)
            {
                if (gameNodes[i].ChildNodes.Count > 4)
                {
                    string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split("(");
                    string awayName = awayDetails[0].Trim();
                    string awayAbr = MatchupDataHelper.GetTeamAbr(awayName);
                    string awayLogo = "/img/nfl-logo/" + awayAbr + ".png";
                    string awayRecord = awayDetails[1].Replace(")", "");
                    string awayScore = gameNodes[i].ChildNodes[7].InnerText;
                    string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                    string homeName = homeDetails[0].Trim();
                    string homeAbr = MatchupDataHelper.GetTeamAbr(homeName);
                    string homeLogo = "/img/nfl-logo/" + homeAbr + ".png";
                    string homeRecord = homeDetails[1].Replace(")", "");
                    string homeScore = gameNodes[i + 1].ChildNodes[7].InnerText;

                    Team awayTeam = new Team(awayName, awayAbr, awayRecord, awayScore, awayLogo);
                    Team homeTeam = new Team(homeName, homeAbr, homeRecord, homeScore, homeLogo);

                    Matchup score = new Matchup(awayTeam, homeTeam, "2019", week);
                    scores.Add(score);
                    //Console.WriteLine("{0} : {1} at {2} : {3}", awayName, awayScore, homeName, homeScore);
                }
                else
                {
                    string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split("(");
                    string awayName = awayDetails[0].Trim();
                    string awayAbr = MatchupDataHelper.GetTeamAbr(awayName);
                    string awayLogo = "/img/nfl-logo/" + awayAbr + ".png";
                    string awayRecord = awayDetails[1].Replace(")", "");
                    string awayScore = gameNodes[i].ChildNodes[3].InnerText;
                    string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                    string homeName = homeDetails[0].Trim();
                    string homeAbr = MatchupDataHelper.GetTeamAbr(homeName);
                    string homeLogo = "/img/nfl-logo/" + homeAbr + ".png";
                    string homeRecord = homeDetails[1].Replace(")", "");
                    string homeScore = gameNodes[i + 1].ChildNodes[3].InnerText;

                    Team awayTeam = new Team(awayName, awayAbr, awayRecord, awayScore, awayLogo);
                    Team homeTeam = new Team(homeName, homeAbr, homeRecord, homeScore, homeLogo);

                    Matchup score = new Matchup(awayTeam, homeTeam, "2019", week);
                    scores.Add(score);
                    //Console.WriteLine("{0} : {1} at {2} : {3}", awayName, awayScore, homeName, homeScore);
                }

            }
            return scores;
        }
        #endregion

        #region

        public static string Get_Team_Record(string teamName)
        {
            string name = Convert_Team_Name(teamName);
            string url = "https://www.footballdb.com/teams/nfl/" + name;
            HtmlWeb page = new HtmlWeb();
            HtmlDocument doc = page.Load(url);

            string banner = doc.DocumentNode.SelectSingleNode("//div[@id='teambanner']//b").InnerText;
            string[] details = banner.Split(new char[] { ':', '(' });

            return details[1];
        }

        #endregion

        #region GET TEAM ABRREVIATION

        public static string GetTeamAbr(string team)
        {
            switch (team)
            {
                case "Arizona":
                    return "ARI";
                case "Atlanta":
                    return "ATL";
                case "Baltimore":
                    return "BAL";
                case "Buffalo":
                    return "BUF";
                case "Carolina":
                    return "CAR";
                case "Chicago":
                    return "CHI";
                case "Cincinnati":
                    return "CIN";
                case "Cleveland":
                    return "CLE";
                case "Dallas":
                    return "DAL";
                case "Denver":
                    return "DEN";
                case "Detroit":
                    return "DET";
                case "Green Bay":
                    return "GB";
                case "Houston":
                    return "HOU";
                case "Indianapolis":
                    return "IND";
                case "Jacksonville":
                    return "JAX";
                case "Kansas City":
                    return "KC";
                case "LA Chargers":
                    return "LAC";
                case "LA Rams":
                    return "LAR";
                case "Miami":
                    return "MIA";
                case "Minnesota":
                    return "MIN";
                case "New England":
                    return "NE";
                case "New Orleans":
                    return "NO";
                case "NY Giants":
                    return "NYG";
                case "NY Jets":
                    return "NYJ";
                case "Oakland":
                    return "OAK";
                case "Philadelphia":
                    return "PHI";
                case "Pittsburgh":
                    return "PIT";
                case "Seattle":
                    return "SEA";
                case "San Francisco":
                    return "SF";
                case "Tampa Bay":
                    return "TB";
                case "Tennessee":
                    return "TEN";
                case "Washington":
                    return "WAS";
            }

            return "";
        }
        #endregion

        #region CONVERT TEAM NAME

        private static string Convert_Team_Name(string teamName)
        {
            switch (teamName)
            {
                case "Buffalo":
                    return "buffalo-bills";
                case "Miami":
                    return "miami-dolphins";
                case "New England":
                    return "new-england-patriots";
                case "Jets":
                    return "new-york-jets";
                case "Ravens":
                    return "baltimore-ravens";
                case "Bengals":
                    return "cincinnati-bengals";
                case "Browns":
                    return "cleveland-browns";
                case "Pittsburgh":
                    return "pittsburgh-steelers";
                case "Houston":
                    return "houston-texans";
                case "Indianapolis":
                    return "indianapolis-colts";
                case "Jacksonville":
                    return "jacksonville-jaguars";
                case "Tennessee":
                    return "tennessee-titans";
                case "Broncos":
                    return "denver-broncos";
                case "Kansas City":
                    return "kansas-city-chiefs";
                case "LA Chargers":
                    return "los-angeles-chargers";
                case "Oakland":
                    return "oakland-raiders";
                case "Cowboys":
                    return "dallas-cowboys";
                case "Giants":
                    return "new-york-giants";
                case "Philadelphia":
                    return "philadelphia-eagles";
                case "Redskins":
                    return "washington-redskins";
                case "Chicago":
                    return "chicago-bears";
                case "Detroit":
                    return "detroit-lions";
                case "Green Bay":
                    return "green-bay-packers";
                case "Minnesota":
                    return "minnesota-vikings";
                case "Atlanta":
                    return "atlanta-falcons";
                case "Panthers":
                    return "carolina-panthers";
                case "New Orleans":
                    return "new-orleans-saints";
                case "Tampa Bay":
                    return "tampa-bay-buccaneers";
                case "Arizona":
                    return "arizona-cardinals";
                case "LA Rams":
                    return "los-angeles-rams";
                case "San Francisco":
                    return "san-francisco-49ers";
                case "Seattle":
                    return "seattle-seahawks";
            }

            return "";
        }

        #endregion
    }
}
