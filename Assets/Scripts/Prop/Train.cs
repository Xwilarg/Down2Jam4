using Down2Jam.Manager;
using Down2Jam.Manager.Persistency;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class Train : MonoBehaviour
    {
        private Vector2 _basePos;
        private Rigidbody2D _rb;

        [SerializeField]
        private AudioSource _sfx;

        private int _step = 0;

        private void Awake()
        {
            _basePos = transform.position;
            _rb = GetComponent<Rigidbody2D>();

            _sfx.volume = PersistencyManager.Instance.SaveData.VolumeSFX;
        }

        private void Update()
        {
            if (TimerManager.Instance.IsActive)
            {
                if (!_sfx.isPlaying) _sfx.Play();
                if (_step == 0)
                {
                    if (TimerManager.Instance.Timer > 3f)
                    {
                        _step++;
                        transform.position = _basePos;
                    }
                }
                else if (_step == 1)
                {
                    if (TimerManager.Instance.Timer > 6f)
                    {
                        _step++;
                        transform.position = _basePos;
                    }
                }

                _rb.linearVelocity = Vector2.left * 30f;
            }
            else
            {
                if (_sfx.isPlaying) _sfx.Stop();
                _rb.linearVelocity = Vector2.zero;
            }
        }

        public void SetVolume(float value)
        {
            _sfx.volume = value;
        }

        public void ResetPos()
        {
            transform.position = _basePos;
            _step = 0;
        }
    }
}
