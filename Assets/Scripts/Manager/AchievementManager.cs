using Down2Jam.Manager.Achievement;
using Down2Jam.Manager.Persistency;
using Down2Jam.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { private set; get; }

        [SerializeField]
        private LevelInfo _firstLevel;

        [SerializeField]
        private Transform _achievementContainer;

        [SerializeField]
        private GameObject _achievementPopupPrefab;

        [SerializeField]
        private Dictionary<AchievementType, AchievementInfo> _achievements;

        private void Awake()
        {
            Instance = this;
        }

        public void Unlock(AchievementType ach)
        {
            if (!PersistencyManager.Instance.SaveData.HasAchievement((int)ach))
            {
                PersistencyManager.Instance.SaveData.UnlockAchievement((int)ach);
                Instantiate(_achievementPopupPrefab, _achievementContainer).GetComponent<AchievementPopup>().Init(_achievements[ach]);
            }
        }

        public void ValidateVictory()
        {
            var allMore800 = true;
            LevelInfo it = _firstLevel;
            do
            {
                var sc = PersistencyManager.Instance.SaveData.GetBestScore(it.name);
                if (sc < 500) return;
                if (sc < 800) allMore800 = false;

                it = it.Next;
            } while (it != null);

            Unlock(AchievementType.WinGame);
            if (allMore800) Unlock(AchievementType.WinAllMoreThan800);
        }
    }

    public enum AchievementType
    {
        WinGame,
        WinAllMoreThan800,
        WinAfter2Explosions
    }
}
