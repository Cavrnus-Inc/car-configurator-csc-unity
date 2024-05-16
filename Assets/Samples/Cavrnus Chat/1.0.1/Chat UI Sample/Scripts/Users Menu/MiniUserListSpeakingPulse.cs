using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class MiniUserListSpeakingPulse : MonoBehaviour
{
    public CanvasGroup cg;
    public bool IsSpeaking;

    public float pulseSpeed = 1f; 
    public float minAlpha = 0.2f; 

    private Coroutine pulsingCoroutine;

    private void Awake()
    {
        cg.alpha = 0f;
    }

    private void Update()
    {
        if (IsSpeaking && pulsingCoroutine == null)
        {
            pulsingCoroutine = StartCoroutine(PulseCanvasGroup());
        }
        else if (!IsSpeaking && pulsingCoroutine != null)
        {
            StopCoroutine(pulsingCoroutine);
            pulsingCoroutine = null;

            cg.alpha = 0f;
        }
    }

    private IEnumerator PulseCanvasGroup()
    {
        var timeOffset = Random.Range(0f, 1f); // Random time offset for variety in pulsing

        while (true)
        {
            // Pulsing effect starting from 0 alpha
            var targetAlpha = minAlpha + Mathf.PingPong((Time.time + timeOffset) * pulseSpeed, 1f - minAlpha);
            cg.alpha = targetAlpha;

            yield return null; // Wait for the next frame
        }
    }
}