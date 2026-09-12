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

        private void Awake()
        {
            Instance = this;

            _victoryPanel.SetActive(false);
        }

        public void ShowVictory(float totalTime, int minipiCount, int totalCount)
        {
            _victoryPanel.SetActive(true);

            var minipiScore = Mathf.CeilToInt((minipiCount / (float)totalCount) * 500f);
            var avrTime = totalTime / minipiCount;
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

            _nextLevelBtn.interactable = finalScore >= 500;
        }

        public void Retry()
        {
            SceneManager.LoadScene("Main");
        }

        public void NextLevel()
        {
            LoaderManager.CurrentLevel = ObjectiveManager.Instance.NextLevel;
            VNManager.SkipIntro = false;
            SceneManager.LoadScene("Main");
        }
    }
}
