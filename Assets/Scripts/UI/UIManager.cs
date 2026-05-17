using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages all UI elements: balance display, result messages, and button state.
/// Listens to GameManager events — no direct coupling to game logic.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Text Displays")]
    public TextMeshProUGUI balanceText;    // Shows current player balance
    public TextMeshProUGUI resultText;     // Shows WIN / LOSE message
    public TextMeshProUGUI betText;        // Shows current bet amount

    [Header("Buttons")]
    public Button spinButton;             // Disabled while spinning

    [Header("Win Panel (Optional Popup)")]
    public GameObject winPanel;           // Panel shown on win (can be null)
    public TextMeshProUGUI winAmountText; // Text inside win panel

    private void Start()
    {
        // Hide result and win panel at start
        if (resultText) resultText.text = "";
        if (winPanel) winPanel.SetActive(false);
    }

    /// <summary>Called by GameManager.onBalanceChanged event.</summary>
    public void UpdateBalance(int newBalance)
    {
        if (balanceText) balanceText.text = $"Balance: {newBalance}";
    }

    /// <summary>Called by GameManager.onWin event.</summary>
    public void ShowWin(int payout)
    {
        if (resultText) resultText.text = $"YOU WIN! +{payout}";

        if (winPanel)
        {
            winPanel.SetActive(true);
            if (winAmountText) winAmountText.text = $"+{payout}";
            StartCoroutine(HideWinPanelAfterDelay(2.5f)); // auto-hide after 2.5 seconds
        }
    }

    private IEnumerator HideWinPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (winPanel) winPanel.SetActive(false);
    }

    /// <summary>Called by GameManager.onLose event.</summary>
    public void ShowLose()
    {
        if (resultText) resultText.text = "Try Again!";
        if (winPanel) winPanel.SetActive(false);
    }

    /// <summary>Called by GameManager.onSpinStarted event — disables spin button.</summary>
    public void OnSpinStarted()
    {
        if (spinButton) spinButton.interactable = false;
        if (resultText) resultText.text = "Spinning...";
        if (winPanel) winPanel.SetActive(false);
    }

    /// <summary>Called by GameManager.onSpinFinished event — re-enables spin button.</summary>
    public void OnSpinFinished()
    {
        if (spinButton) spinButton.interactable = true;
    }

    /// <summary>Updates the bet display text.</summary>
    public void UpdateBetDisplay(int bet)
    {
        if (betText) betText.text = $"Bet: {bet}";
    }
}
