using Assets.Scripts.Manager;
using UnityEngine;

namespace Down2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/AchievementInfo", fileName = "AchievementInfo")]
    public class AchievementInfo : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Icon;
    }
}