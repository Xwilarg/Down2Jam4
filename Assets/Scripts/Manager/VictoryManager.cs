using Assets.Scripts.Manager;
using Down2Jam.Manager.Persistency;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Down2Jam.Manager
{
    public class VictoryManager : MonoBehaviour
    {
        public static VictoryManager Instance { private set; get; }

        [SerializeField]
        private GameObject _victoryPanel;

        [SerializeField]
        private TMP_Text _finalText;

        [SerializeField]
        private Button _nextLevelBtn;

        public bool IsGameFinished { set; get; }

        private void Awake()
        {
            Instance = this;

            _victoryPanel.SetActive(false);
        }

        public int ShowVictory(float totalTime, int minipiCount, int totalCount)
        {
            if (IsGameFinished) return -1;

            IsGameFinished = true;

            _victoryPanel.SetActive(true);

            var minipiScore = totalCount == 0 ? 0 : (Mathf.CeilToInt((minipiCount / (float)totalCount) * 500f));
            var avrTime = minipiCount == 0 ? 10 : (totalTime / minipiCount);
            var timeScore = Mathf.CeilToInt(500 - (avrTime * 500f / 10f));
            var finalScore = timeScore + minipiScore;

            var levelName = LoaderManager.CurrentLevel.name;
            PersistencyManager.Instance.SaveData.SaveScore(levelName, finalScore);

            _finalText.text =
                $"Minipi Delivered: {minipiCount}\n" +
                $"+{minipiScore}\n" +
                $"\n" +
                $"Average Time: {avrTime:0.0}s\n" +
                $"+{timeScore}\n" +
                $"\n" +
                $"<b>Final Score: {finalScore} / 500</b>\n" +
                $"Best Score: {PersistencyManager.Instance.SaveData.GetBestScore(levelName)}";

            if (LoaderManager.CurrentLevel.Next == null)
            {
                _nextLevelBtn.gameObject.SetActive(false);
                AchievementManager.Instance.ValidateVictory();
            }
            else
            {
                _nextLevelBtn.interactable = finalScore >= 500;
            }

            return finalScore;
        }

        public void Retry()
        {
            SceneManager.LoadScene("Main");
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void NextLevel()
        {
            LoaderManager.CurrentLevel = ObjectiveManager.Instance.NextLevel;
            VNManager.SkipIntro = false;
            SceneManager.LoadScene("Main");
        }
    }
}
