using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RankingNBATeams.App
{
    public class CalculateTheRankOfNBATeam : ICalculateTheRankOfNBATeam
    {
        public string NbaCup(string resultSheet, string toFind)
        {

            string[] games = resultSheet.Split(",");

            int wins = 0;
            int draws = 0;
            int losses = 0;
            int pointsScoredByTeam = 0;
            int pointsConcededByTeam = 0;
            int teamPoints = 0;
            int gamesPlayed = 0;
            int errorScore = 0;
            List<string> errorTeams = new List<string>();
            List<string> errorTeamScores = new List<string>();

            foreach (var game in games)
            {
                var game1 = $"{game} ";
                string[] teamsAndScores = Regex.Split(game1, @"(\d+\W?\d+)\s");
                teamsAndScores = teamsAndScores.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                var team1 = teamsAndScores[0].Trim();
                var team2 = teamsAndScores[2].Trim();
                var score1 = teamsAndScores[1].Trim();
                var score2 = teamsAndScores[3].Trim();

                if (score1.Contains(".") || score2.Contains("."))
                {
                    errorScore += 1;
                    errorTeams.Add(team1);
                    errorTeams.Add(team2);

                    errorTeamScores.Add(score1);
                    errorTeamScores.Add(score2);
                    break;
                }

                if (team1.Equals(toFind))
                {
                    pointsScoredByTeam += int.Parse(score1);
                    pointsConcededByTeam += int.Parse(score2);

                    gamesPlayed += 1;
                    if (int.Parse(score1) > int.Parse(score2))
                    {
                        wins += 1;
                        teamPoints += 3;
                    }
                    else if (int.Parse(score1) < int.Parse(score2))
                    {
                        losses += 1;
                    }
                    else
                    {
                        draws += 1;
                        teamPoints += 1;
                    }
                }

                else if (team2.Equals(toFind))
                {
                    gamesPlayed += 1;
                    pointsScoredByTeam += int.Parse(score2);
                    pointsConcededByTeam += int.Parse(score1);

                    if (int.Parse(score1) > int.Parse(score2))
                    {
                        losses += 1;
                    }
                    else if (int.Parse(score1) < int.Parse(score2))
                    {
                        wins += 1;
                        teamPoints += 3;
                    }
                    else
                    {
                        draws += 1;
                        teamPoints += 1;
                    }
                }

                else
                {
                    gamesPlayed += 0;
                }
            }
            
            if(!string.IsNullOrEmpty(toFind))
            {
                if(errorScore == 1)
                {
                    return $"Error(float number):{errorTeams[0]} {errorTeamScores[0]} {errorTeams[1]} {errorTeamScores[1]}";
                }

                if (gamesPlayed == 0)
                {
                    return ($"{toFind}:This team didn't play!");
                }

                return ($"{toFind}:W={wins};D={draws};L={losses};Scored={pointsScoredByTeam};Conceded={pointsConcededByTeam};Points={teamPoints}");
            }
            return "";
        }
    }
}
