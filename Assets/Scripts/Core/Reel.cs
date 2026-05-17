using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a single reel using a vertical strip of symbol Images.
/// Spins by rapidly shuffling sprites across the strip, then lands on a final RNG result.
/// Designed for a VerticalLayoutGroup with 4 child Image slots.
/// </summary>
public class Reel : MonoBehaviour
{
    [Header("Symbol Strip — assign all 4 Image children in order (top to bottom)")]
    public Image[] symbolImages;      // Drag all 4 child Images here in Inspector

    [Header("Symbol Data")]
    public SlotSymbol[] symbols;      // Fill with 4 SlotSymbol entries (Cherry, Bell, Star, Seven)

    [Header("Spin Settings")]
    public float spinDuration = 2f;
    public float symbolFlipInterval = 0.08f;

    // The index of the symbol currently shown in the CENTER (index 1) slot
    private int currentIndex;
    private RNGManager rng;
    private ReelSpinAnimator bounceAnimator;

    private void Awake()
    {
        rng = FindObjectOfType<RNGManager>();
        bounceAnimator = GetComponent<ReelSpinAnimator>();
    }

    /// <summary>
    /// Spins the reel: rapidly randomizes all strip images, then lands on a final result.
    /// </summary>
    public IEnumerator Spin(float delay = 0f)
    {
        // Staggered start — each reel waits a bit longer than the previous
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;

        // Rapid shuffle: randomize all strip images quickly to simulate spinning
        while (elapsed < spinDuration)
        {
            foreach (Image img in symbolImages)
            {
                img.sprite = symbols[rng.GetRandomIndex(symbols.Length)].symbolSprite;
            }
            elapsed += symbolFlipInterval;
            yield return new WaitForSeconds(symbolFlipInterval);
        }

        // Land: pick final symbol, show it in center slot (index 1)
        currentIndex = rng.GetRandomIndex(symbols.Length);

        // Fill the strip around the landed symbol for a natural look
        // Top slot: symbol before, Center: landed symbol, Bottom slots: symbols after
        for (int i = 0; i < symbolImages.Length; i++)
        {
            int symbolIndex = (currentIndex + i) % symbols.Length;
            symbolImages[i].sprite = symbols[symbolIndex].symbolSprite;
        }

        // Play bounce animation if ReelSpinAnimator is attached
        if (bounceAnimator != null)
            yield return StartCoroutine(bounceAnimator.TriggerBounce());
    }

    /// <summary>Returns the symbol the reel landed on (center slot).</summary>
    public SlotSymbol GetCurrentSymbol()
    {
        return symbols[currentIndex];
    }
}
