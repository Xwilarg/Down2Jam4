using System.Collections.Generic;

namespace Down2Jam.Manager.Persistency
{
    public class SaveData
    {
        public Dictionary<string, LevelData> Levels { set; get; } = new();

        public void SaveScore(string level, int score)
        {
            if (Levels.TryGetValue(level, out var data))
            {
                if (score > data.BestScore)
                {
                    data.BestScore = score;
                }
                PersistencyManager.Instance.Save();
            }
            else
            {
                Levels.Add(level, new()
                {
                    BestScore = score
                });
                PersistencyManager.Instance.Save();
            }
        }

        public int GetBestScore(string level)
        {
            if (Levels.TryGetValue(level, out var data)) return data.BestScore;
            return 0;
        }
    }

    public class LevelData
    {
        public int BestScore { set; get; }
    }
}
