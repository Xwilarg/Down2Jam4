using UnityEngine;

namespace Down2Jam.UI
{
    public class Sticker : MonoBehaviour
    {
        private float _timer = 2f;

        private void Awake()
        {
            transform.rotation = Quaternion.Euler(0, 0, 30f);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0f) _timer += 2f;

            transform.rotation = Quaternion.Euler(0, 0, _timer < 1f ? -30f : 30f);
        }
    }
}
