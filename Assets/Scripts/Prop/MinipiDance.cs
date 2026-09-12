using System.Collections;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class MinipiDance : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(Dance());
        }

        private IEnumerator Dance()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                transform.localScale = new(-transform.localScale.x, 1f, 1f);
            }
        }
    }
}
