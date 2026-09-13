using Down2Jam.Manager.Achievement;
using Down2Jam.Manager.Persistency;
using Down2Jam.SO;
using System.Collections.Generic;
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
        public Dictionary<AchievementType, AchievementInfo> Achievements => _achievements;

        private void Awake()
        {
            Instance = this;
        }

        public void Unlock(AchievementType ach)
        {
            if (!PersistencyManager.Instance.SaveData.HasAchievement((int)ach))
            {
                PersistencyManager.Instance.SaveData.UnlockAchievement((int)ach);
                Instantiate(_achievementPopupPrefab, _achievementContainer).GetComponent<AchievementPopup>().InitEarned(_achievements[ach]);
            }
        }

        public void ValidateVictory()
        {
            int count800 = 0;
            int victoryCount = 0;
            LevelInfo it = _firstLevel;
            do
            {
                var sc = PersistencyManager.Instance.SaveData.GetBestScore(it.name);
                if (sc >= 500) victoryCount++;
                if (sc >= 800) count800++;

                it = it.Next;
            } while (it != null);

            if (victoryCount >= 8) Unlock(AchievementType.WinGame);
            if (count800 >= 4) Unlock(AchievementType.FourWinMoreThan800);
            if (count800 >= 8) Unlock(AchievementType.EightWinMoreThan800);
        }
    }

    public enum AchievementType
    {
        WinGame,
        EightWinMoreThan800,
        WinAfter2Explosions,
        SecretRap,
        TrainSoLong,
        FourWinMoreThan800
    }
}
