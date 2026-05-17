using System.Collections;
using UnityEngine;

/// <summary>
/// Bonus Feature: Adds a vertical bounce effect to a reel when it stops spinning.
/// Attach to each Reel GameObject alongside the Reel script.
/// Call TriggerBounce() from Reel.Spin() after landing.
/// </summary>
public class ReelSpinAnimator : MonoBehaviour
{
    public float bounceDistance = 10f;  // pixels to bounce down then snap up
    public float bounceDuration = 0.15f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    /// <summary>Call this after the reel lands to play the bounce.</summary>
    public IEnumerator TriggerBounce()
    {
        // Move down slightly
        float t = 0f;
        while (t < bounceDuration)
        {
            t += Time.deltaTime;
            float offset = Mathf.Lerp(0, -bounceDistance, t / bounceDuration);
            rectTransform.anchoredPosition = originalPosition + new Vector2(0, offset);
            yield return null;
        }

        // Snap back up
        t = 0f;
        while (t < bounceDuration)
        {
            t += Time.deltaTime;
            float offset = Mathf.Lerp(-bounceDistance, 0, t / bounceDuration);
            rectTransform.anchoredPosition = originalPosition + new Vector2(0, offset);
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }
}
