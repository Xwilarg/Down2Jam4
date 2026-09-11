using Ink.Runtime;
using Ink.UnityIntegration;
using NsfwDelivery.SO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Down2Jam.Manager
{
    public class VNManager : MonoBehaviour
    {
        public static VNManager Instance { private set; get; }

        [SerializeField]
        private InkFile _intro;

        [SerializeField]
        private GameObject _vnContainer;

        [SerializeField]
        private GameObject _nameContainer;

        [SerializeField]
        private TMP_Text _mainText;

        [SerializeField]
        private TMP_Text _name, _nameSubtitle;

        [SerializeField]
        private Image _sticker, _body;

        [SerializeField]
        private VNSpeakerInfo[] _speakers;

        private Story _story;

        public bool IsPlayingStory => _vnContainer.activeInHierarchy;

        private void Awake()
        {
            Instance = this;

            _story = new(_intro.storyJson);
            _nameContainer.SetActive(false);
            _sticker.gameObject.SetActive(false);
            _body.gameObject.SetActive(false);
            DisplayNextDialogue();
        }

        private void UpdateDisplay()
        {
            _mainText.text = _story.currentText;

            foreach (var tag in _story.currentTags)
            {
                var parts = tag.ToLowerInvariant().Split(' ');
                var body = string.Join(" ", parts.Skip(1));
                switch (parts[0])
                {
                    case "speaker":
                        var target = _speakers.FirstOrDefault(x => x.ID == body);
                        if (target != null)
                        {
                            _nameContainer.SetActive(true);
                            _name.text = target.Name;
                            _nameSubtitle.text = target.Subtitle;
                            _body.gameObject.SetActive(target.BodySprite != null);
                            _body.sprite = target.BodySprite;
                            _sticker.sprite = target.StickerSprite;
                        }
                        else
                        {
                            _nameContainer.SetActive(false);
                            _body.gameObject.SetActive(false);
                        }
                        break;

                    case "sticker": _sticker.gameObject.SetActive(body == "on"); break;

                    case "time": break;

                    default:
                        Debug.LogWarning($"Unknown story tag {parts[0]}");
                        break;
                }
            }
        }

        public void DisplayNextDialogue()
        {
            if (_story.canContinue)
            {
                _story.Continue();
                UpdateDisplay();
            }
            else
            {
                _vnContainer.SetActive(false);
            }
        }
    }
}
