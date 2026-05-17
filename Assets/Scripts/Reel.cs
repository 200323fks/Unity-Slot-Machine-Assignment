using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public Image symbolDisplay;
    public SlotSymbol[] symbols;
    public float spinDuration = 2f;

    private int currentIndex;

    public IEnumerator Spin()
    {
        float time = 0f;

        while (time < spinDuration)
        {
            int randomIndex = Random.Range(0, symbols.Length);
            symbolDisplay.sprite = symbols[randomIndex].symbolSprite;
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        currentIndex = Random.Range(0, symbols.Length);
        symbolDisplay.sprite = symbols[currentIndex].symbolSprite;
    }

    public SlotSymbol GetCurrentSymbol()
    {
        return symbols[currentIndex];
    }
}