using System.Collections;
using UnityEngine;
using UnityEngine.UI;


// Controls a single reel using a vertical strip of symbol Images.

public class Reel : MonoBehaviour
{
    [Header("Symbol Strip — assign all 4 Image children in order (top to bottom)")]
    public Image[] symbolImages;      // Drag all 4 child Images here in Inspector

    [Header("Symbol Data")]
    public SlotSymbol[] symbols;      // Fill with 4 SlotSymbol entries (Cherry, Bell,Bar, Seven)

    [Header("Spin Settings")]
    public float spinDuration = 2f;
    public float symbolFlipInterval = 0.08f;

    
    private int currentIndex;
    private RNGManager rng;
    private ReelSpinAnimator bounceAnimator;

    private void Awake()
    {
        rng = FindObjectOfType<RNGManager>();
        bounceAnimator = GetComponent<ReelSpinAnimator>();
    }

   
    public IEnumerator Spin(float delay = 0f)
    {
              yield return new WaitForSeconds(delay);

        float elapsed = 0f;

       
        while (elapsed < spinDuration)
        {
            foreach (Image img in symbolImages)
            {
                img.sprite = symbols[rng.GetRandomIndex(symbols.Length)].symbolSprite;
            }
            elapsed += symbolFlipInterval;
            yield return new WaitForSeconds(symbolFlipInterval);
        }

        currentIndex = rng.GetRandomIndex(symbols.Length);

        for (int i = 0; i < symbolImages.Length; i++)
        {
            int symbolIndex = (currentIndex + i) % symbols.Length;
            symbolImages[i].sprite = symbols[symbolIndex].symbolSprite;
        }

        if (bounceAnimator != null)
            yield return StartCoroutine(bounceAnimator.TriggerBounce());
    }

    public SlotSymbol GetCurrentSymbol()
    {
        return symbols[currentIndex];
    }
}
