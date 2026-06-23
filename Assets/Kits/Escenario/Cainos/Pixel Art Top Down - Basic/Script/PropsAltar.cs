using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;

        private int overlapCount = 0;

        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            overlapCount++;
            targetColor.a = 1.0f;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            overlapCount = Mathf.Max(0, overlapCount - 1);

            if (overlapCount == 0)
            {
                targetColor.a = 0.0f;
            }
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }
    }
}
