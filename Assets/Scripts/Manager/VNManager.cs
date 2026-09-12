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

        public static bool SkipIntro { set; get; } = false;

        [SerializeField]
        private GameObject _vnContainer;

        [SerializeField]
        private GameObject _nameContainer;

        [SerializeField]
        private TMP_Text _mainText;

        [SerializeField]
        private TMP_Text _name, _nameSubtitle;

        [SerializeField]
        private GameObject _stickerContainer;

        [SerializeField]
        private Image _sticker, _body, _emotion;

        [SerializeField]
        private VNSpeakerInfo[] _speakers;

        private VNSpeakerInfo _currSpeaker;

        private Story _story;

        public bool IsPlayingStory => _vnContainer.activeInHierarchy;

        private void Awake()
        {
            Instance = this;
            _vnContainer.SetActive(false);
            _nameContainer.SetActive(false);
            _stickerContainer.SetActive(false);
            _body.gameObject.SetActive(false);
        }

        public void PlayStory(InkFile inkFile)
        {
            if (!SkipIntro)
            {
                _vnContainer.SetActive(true);
                _story = new(inkFile.storyJson);
                DisplayNextDialogue();
                SkipIntro = true;
            }
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
                        _currSpeaker = _speakers.FirstOrDefault(x => x.ID == body);
                        if (_currSpeaker != null)
                        {
                            _nameContainer.SetActive(true);
                            _name.text = _currSpeaker.Name;
                            _nameSubtitle.text = _currSpeaker.Subtitle;
                            _body.gameObject.SetActive(_currSpeaker.BodySprite != null);
                            _body.sprite = _currSpeaker.BodySprite;
                            _sticker.sprite = _currSpeaker.StickerSprite;
                            _emotion.gameObject.SetActive(false);
                        }
                        else
                        {
                            if (body != "none") Debug.LogWarning($"Unknown speaker {body}");
                            _nameContainer.SetActive(false);
                            _body.gameObject.SetActive(false);
                            _emotion.gameObject.SetActive(false);
                        }
                        break;

                    case "sticker": _stickerContainer.SetActive(body == "on"); break;

                    case "time": break;

                    case "emotion":
                        if (_currSpeaker.Emotions.TryGetValue(body, out var emotion))
                        {
                            _emotion.gameObject.SetActive(true);
                            _emotion.sprite = emotion;
                        }
                        else
                            _emotion.gameObject.SetActive(false);
                        break;

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
