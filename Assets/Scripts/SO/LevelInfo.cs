using Eflatun.SceneReference;
using Ink.UnityIntegration;
using UnityEngine;

namespace Down2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelInfo", fileName = "LevelInfo")]
    public class LevelInfo : ScriptableObject
    {
        public SceneReference Level;
        public OrderInfo[] Orders;
        public InkFile Intro;
        public LevelInfo Next;
    }
}