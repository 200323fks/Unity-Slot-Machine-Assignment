using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a single reel: spinning animation and final symbol selection.
/// Uses RNGManager for fair randomization.
/// </summary>
public class Reel : MonoBehaviour
{
    [Header("References")]
    public Image symbolDisplay;       // The UI Image showing the current symbol
    public SlotSymbol[] symbols;      // All possible symbols on this reel

    [Header("Spin Settings")]
    public float spinDuration = 2f;   // How long the reel spins before stopping
    public float symbolFlipInterval = 0.08f; // How fast symbols flash during spin

    private int currentIndex;
    private RNGManager rng;
    private ReelSpinAnimator bounceAnimator; // optional bounce effect

    private void Awake()
    {
        rng = FindObjectOfType<RNGManager>();
        bounceAnimator = GetComponent<ReelSpinAnimator>();
    }

    /// <summary>
    /// Spins the reel: rapidly flips symbols, then lands on a final RNG result.
    /// </summary>
    public IEnumerator Spin(float delay = 0f)
    {
        // Staggered start delay for cascading reel effect
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;

        // Rapid symbol flipping to simulate spinning
        while (elapsed < spinDuration)
        {
            int randomIndex = rng.GetRandomIndex(symbols.Length);
            symbolDisplay.sprite = symbols[randomIndex].symbolSprite;
            elapsed += symbolFlipInterval;
            yield return new WaitForSeconds(symbolFlipInterval);
        }

        // Land on final symbol
        currentIndex = rng.GetRandomIndex(symbols.Length);
        symbolDisplay.sprite = symbols[currentIndex].symbolSprite;

        // Play bounce animation if available
        if (bounceAnimator != null)
            yield return StartCoroutine(bounceAnimator.TriggerBounce());
    }

    /// <summary>Returns the symbol the reel landed on.</summary>
    public SlotSymbol GetCurrentSymbol()
    {
        return symbols[currentIndex];
    }
}
