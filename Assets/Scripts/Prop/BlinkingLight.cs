using System.Collections;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class BlinkingLight : MonoBehaviour
    {
        [SerializeField]
        private Color _color1, _color2;

        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _sr.color = _color1;

            StartCoroutine(Blink());
        }

        private IEnumerator Blink()
        {
            var wait = new WaitForSeconds(.5f);

            while (true)
            {
                _sr.color = _color1;
                yield return wait;
                _sr.color = _color2;
                yield return wait;
            }
        }
    }
}
