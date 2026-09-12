using Eflatun.SceneReference;
using Ink.UnityIntegration;
using UnityEngine;

namespace NsfwDelivery.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelInfo", fileName = "LevelInfo")]
    public class LevelInfo : ScriptableObject
    {
        public SceneReference Level;
        public OrderInfo[] Orders;
        public InkFile Intro;
    }
}