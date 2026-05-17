using System.Collections;
using UnityEngine;


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
       
        StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        
        float t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0f, popScale, t / popDuration);
            transform.localScale = originalScale * scale;
            yield return null;
        }

       
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
