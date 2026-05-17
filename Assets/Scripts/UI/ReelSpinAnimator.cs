using System.Collections;
using UnityEngine;


public class ReelSpinAnimator : MonoBehaviour
{
    public float bounceDistance = 10f; 
    public float bounceDuration = 0.15f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

   
    public IEnumerator TriggerBounce()
    {
      
        float t = 0f;
        while (t < bounceDuration)
        {
            t += Time.deltaTime;
            float offset = Mathf.Lerp(0, -bounceDistance, t / bounceDuration);
            rectTransform.anchoredPosition = originalPosition + new Vector2(0, offset);
            yield return null;
        }

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
