using Palcikas_Jatek.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Palcikas_Jatek.Repositories
{
    public static class ScoresRepository
    {
        public static IList<Score> GetScores()
        {
            var appDataPath = GetAppDataPath();

            if (File.Exists(appDataPath))
            {
                var rawContent = File.ReadAllText(appDataPath);
                var scores = JsonSerializer.Deserialize<IList<Score>>(rawContent);
                return scores;
            }

            return new List<Score>();
        }

        public static void StoreScores(IList<Score> scores)
        {
            var appDataPath = GetAppDataPath();

            var rawContent = JsonSerializer.Serialize(scores);
            File.WriteAllText(appDataPath, rawContent);
        }

        public static void StoreScore(Score score)
        {
            var scores = GetScores();
            int maxSquare = 0;
            int maxRombus = 0;
            foreach (var sc in scores)
            {
                if (sc.Square)
                {
                    if(sc.Value > maxSquare)
                    {
                        maxSquare = sc.Value;
                    }
                }
                else
                {
                    if (sc.Value > maxRombus)
                    {
                        maxRombus = sc.Value;
                    }
                }
            }
            if (score.Square)
            {
                if (score.Value >= maxSquare)
                {
                    scores.Add(score);
                }
            }
            else
            {
                if(score.Value >= maxRombus)
                {
                    scores.Add(score);
                }
            }
           
            StoreScores(scores);
        }


        public static string GetAppDataPath()
        {
            var localAppFolder = GetLocalFolder();
            var appDataPath = Path.Combine(localAppFolder, "Scores.json");
            return appDataPath;
        }

        public static string GetLocalFolder()
        {
            var localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var localAppFolder = Path.Combine(localAppDataFolder, "Palcikas_Jatek");

            if (!Directory.Exists(localAppFolder))
            {
                Directory.CreateDirectory(localAppFolder);
            }

            return localAppFolder;
        }
    }
}
