using System.Collections.Generic;
using UnityEngine;

namespace Down2Jam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/VNSpeakerInfo", fileName = "VNSpeakerInfo")]
    public class VNSpeakerInfo : ScriptableObject
    {
        public string ID;
        public string Name;
        public string Subtitle;

        public Sprite BodySprite;
        public Sprite StickerSprite;

        [SerializeField]
        public Dictionary<string, Sprite> Emotions;
    }
}