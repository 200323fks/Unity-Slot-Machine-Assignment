using System.Collections;
using UnityEngine;

/// <summary>
/// Animates the slot machine handle: rotates 90 degrees on click, returns after 1 second.
/// Attach this to the Handle_Image GameObject.
/// Call TriggerPull() from the SpinButton's OnClick or from GameManager.StartSpin.
/// </summary>
public class HandleAnimator : MonoBehaviour
{
    public float rotateDuration = 0.2f;  // how fast it rotates to 90 degrees
    public float holdDuration = 0.6f;    // how long it stays pulled before returning
    public float returnDuration = 0.2f;  // how fast it returns to original position

    private RectTransform rectTransform;
    private Quaternion originalRotation;
    private bool isAnimating = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalRotation = rectTransform.localRotation;
    }

    /// <summary>Call this when the spin button is clicked.</summary>
    public void TriggerPull()
    {
        if (isAnimating) return;
        StartCoroutine(PullAnimation());
    }

    private IEnumerator PullAnimation()
    {
        isAnimating = true;

        Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, 0f, -90f);

        // Rotate down to 90 degrees
        float t = 0f;
        while (t < rotateDuration)
        {
            t += Time.deltaTime;
            rectTransform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, t / rotateDuration);
            yield return null;
        }
        rectTransform.localRotation = targetRotation;

        // Hold at pulled position
        yield return new WaitForSeconds(holdDuration);

        // Return to original rotation
        t = 0f;
        while (t < returnDuration)
        {
            t += Time.deltaTime;
            rectTransform.localRotation = Quaternion.Lerp(targetRotation, originalRotation, t / returnDuration);
            yield return null;
        }
        rectTransform.localRotation = originalRotation;

        isAnimating = false;
    }
}
