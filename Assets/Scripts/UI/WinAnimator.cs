using System.Collections;
using UnityEngine;

/// <summary>
/// Bonus Feature: Plays a scale-pop animation on the win panel when player wins.
/// Attach this to the WinPanel GameObject.
/// </summary>
public class WinAnimator : MonoBehaviour
{
    public float popDuration = 0.3f;
    public float popScale = 1.2f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        // Trigger pop animation every time the panel is shown
        StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        // Scale up
        float t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0f, popScale, t / popDuration);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        // Scale back to normal
        t = 0f;
        while (t < popDuration * 0.5f)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(popScale, 1f, t / (popDuration * 0.5f));
            transform.localScale = originalScale * scale;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
