using Down2Jam.Manager.Persistency;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class VictoryManager : MonoBehaviour
    {
        public static VictoryManager Instance { private set; get; }

        [SerializeField]
        private GameObject _victoryPanel;

        [SerializeField]
        private TMP_Text _finalText;

        private void Awake()
        {
            Instance = this;

            _victoryPanel.SetActive(false);
        }

        public void ShowVictory(float totalTime, int minipiCount)
        {
            _victoryPanel.SetActive(true);

            var avrTime = totalTime / minipiCount;
            var timeScore = Mathf.FloorToInt(500 - (avrTime * 500f / 10f));
            var finalScore = timeScore + 500;

            var levelName = LoaderManager.CurrentLevel.name;
            PersistencyManager.Instance.SaveData.SaveScore(levelName, finalScore);

            _finalText.text =
                $"Minipi Delivered: {minipiCount}\n" +
                $"+500\n" +
                $"\n" +
                $"Average Time: {avrTime:0.0}s\n" +
                $"+{timeScore}\n" +
                $"\n" +
                $"<b>Final Score: {finalScore}</b>\n" +
                $"Best Score: {PersistencyManager.Instance.SaveData.GetBestScore(levelName)}";
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
