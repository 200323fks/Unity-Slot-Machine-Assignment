using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    [Header("Text Displays")]
    public TextMeshProUGUI balanceText;    // Shows current player balance
    public TextMeshProUGUI resultText;     // Shows WIN / LOSE message
    public TextMeshProUGUI betText;        // Shows current bet amount

    [Header("Buttons")]
    public Button spinButton;            

    [Header("Win Panel (Optional Popup)")]
    public GameObject winPanel;           
    public TextMeshProUGUI winAmountText;

    private void Start()
    {
        
        if (resultText) resultText.text = "";
        if (winPanel) winPanel.SetActive(false);
    }

   
    public void UpdateBalance(int newBalance)
    {
        if (balanceText) balanceText.text = $"Balance: {newBalance}";
    }

   
    public void ShowWin(int payout)
    {
        if (resultText) resultText.text = $"YOU WIN! +{payout}";

        if (winPanel)
        {
            winPanel.SetActive(true);
            if (winAmountText) winAmountText.text = $"+{payout}";
            StartCoroutine(HideWinPanelAfterDelay(2.5f)); 
        }
    }

    private IEnumerator HideWinPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (winPanel) winPanel.SetActive(false);
    }

    
    public void ShowLose()
    {
        if (resultText) resultText.text = "Try Again!";
        if (winPanel) winPanel.SetActive(false);
    }

   
    public void ShowGameOver()
    {
        if (winPanel)
        {
            winPanel.SetActive(true);
            if (winAmountText) winAmountText.text = "YOU LOSE";
        }
        if (resultText) resultText.text = "Game Over!";
        if (spinButton) spinButton.interactable = false;
    }

   
    public void ShowWinner()
    {
        if (winPanel)
        {
            winPanel.SetActive(true);
            if (winAmountText) winAmountText.text = "WINNER!";
        }
        if (resultText) resultText.text = "Amazing! Resetting...";
        if (spinButton) spinButton.interactable = false;
    }

   
    public void OnSpinStarted()
    {
        if (spinButton) spinButton.interactable = false;
        if (resultText) resultText.text = "Spinning...";
        if (winPanel) winPanel.SetActive(false);
    }

    public void OnSpinFinished()
    {
        if (spinButton) spinButton.interactable = true;
        if (winPanel) winPanel.SetActive(false);  // hide end-game popup on reset
        if (resultText) resultText.text = "";
    }

    /// <summary>Updates the bet display text.</summary>
    public void UpdateBetDisplay(int bet)
    {
        if (betText) betText.text = $"Bet: {bet}";
    }
}
