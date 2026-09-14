using Assets.Scripts.Manager;
using Down2Jam.Manager.Achievement;
using Down2Jam.Manager.Persistency;
using Down2Jam.Prop;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Down2Jam.Manager
{
    public class GlobalUIManager : MonoBehaviour
    {
        public static GlobalUIManager Instance { private set; get; }

        [SerializeField]
        private GameObject _achievementPanel, _settingsPanel;

        [SerializeField]
        private Transform _achievementPanelContainer;

        [SerializeField]
        private GameObject _achievementPrefab;

        [SerializeField]
        private AudioSource _bgm, _sfx;

        [SerializeField]
        private Slider _volumeSlider, _sfxVolumeSlider;

        [SerializeField]
        private TMP_Text _deleteSaveText;

        private bool _deleteSavesConfirm = false;

        private void Awake()
        {
            Instance = this;

            _achievementPanel.SetActive(false);
            _settingsPanel.SetActive(false);

            _volumeSlider.value = PersistencyManager.Instance.SaveData.Volume;
            _sfxVolumeSlider.value = PersistencyManager.Instance.SaveData.VolumeSFX;
            _bgm.volume = _volumeSlider.value;
            _sfx.volume = _sfxVolumeSlider.value;
        }

        public void SetBGM(AudioSource source)
        {
            _bgm.Stop();
            _bgm = source;
            _bgm.volume = PersistencyManager.Instance.SaveData.Volume;
        }

        public void DeleteSaves()
        {
            if (!_deleteSavesConfirm)
            {
                _deleteSavesConfirm = true;
                _deleteSaveText.text = "Are you sure?";
            }
            else
            {
                PersistencyManager.Instance.ClearSaves();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void OnVolumeChange(float volume)
        {
            PersistencyManager.Instance.SaveData.Volume = volume;
            _bgm.volume = volume;
        }

        public void OnSFXVolumeChange(float volume)
        {
            PersistencyManager.Instance.SaveData.VolumeSFX = volume;
            _sfx.volume = volume;
            ForkliftManager.Instance?.Train?.SetVolume(volume);
        }

        public void ToggleAchievement()
        {
            _achievementPanel.SetActive(!_achievementPanel.activeInHierarchy);

            if (_achievementPanel.activeInHierarchy)
            {
                for (int i = _achievementPanelContainer.childCount - 1; i >= 0; i--) Destroy(_achievementPanelContainer.GetChild(i).gameObject);
                foreach (var ach in AchievementManager.Instance.Achievements)
                {
                    var inst = Instantiate(_achievementPrefab, _achievementPanelContainer);
                    inst.GetComponent<AchievementPopup>().InitUI(ach.Value, PersistencyManager.Instance.SaveData.HasAchievement((int)ach.Key));
                }
            }
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ToggleSettings()
        {
            _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy);
            if (!_settingsPanel.activeInHierarchy)
            {
                _deleteSavesConfirm = false;
                PersistencyManager.Instance.Save();
            }
        }
    }
}
