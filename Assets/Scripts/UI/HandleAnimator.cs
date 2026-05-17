using System.Collections;
using UnityEngine;


public class HandleAnimator : MonoBehaviour
{
    public float rotateDuration = 0.2f;  
    public float holdDuration = 0.6f;    
    public float returnDuration = 0.2f;  

    private RectTransform rectTransform;
    private Quaternion originalRotation;
    private bool isAnimating = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalRotation = rectTransform.localRotation;
    }

  
    public void TriggerPull()
    {
        if (isAnimating) return;
        StartCoroutine(PullAnimation());
    }

    private IEnumerator PullAnimation()
    {
        isAnimating = true;

        Quaternion targetRotation = originalRotation * Quaternion.Euler(50f, 0f, 0f);

       
        float t = 0f;
        while (t < rotateDuration)
        {
            t += Time.deltaTime;
            rectTransform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, t / rotateDuration);
            yield return null;
        }
        rectTransform.localRotation = targetRotation;

      
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
