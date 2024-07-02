using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cavrnus.Chat
{
    public static class AnimationHelper
    {
        public static IEnumerator DoFade(this MonoBehaviour go, List<CanvasGroup> cgs, float duration, bool fadeIn)
        {
            var start = fadeIn ? 0f : 1f;
            var end = fadeIn ? 1f : 0f;
            cgs.ForEach(cg => cg.alpha = fadeIn ? 0f : 1f);
            
            var elapsedTime = 0f;
            while (elapsedTime < duration) {
                if (go == null || go.Equals(null))
                    yield break;
                
                var normalizedTime = elapsedTime / duration;
                cgs.ForEach(cg => cg.alpha = Mathf.Lerp(start, end,normalizedTime));

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            
            cgs.ForEach(cg => cg.alpha = fadeIn ? 1f : 0f);
        }
    }
}