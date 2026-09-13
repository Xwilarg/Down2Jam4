using UnityEngine;

namespace Down2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/AchievementInfo", fileName = "AchievementInfo")]
    public class AchievementInfo : ScriptableObject
    {
        public string Name;
        public string UnlockExpl;
        public Sprite Icon;
    }
}