using System.Collections;
using UnityEngine;
using UnityEngine.Events;


// Manages spin flow, balance, and win/lose logic.

public class GameManager : MonoBehaviour
{
    [Header("Reels")]
    public Reel[] reels;             

    [Header("Handle")]
    public HandleAnimator handleAnimator;  

    [Header("Balance Settings")]
    public int playerBalance = 100;
    public int betAmount = 10;      

    [Header("Stagger Settings")]
    public float reelStaggerDelay = 0.2f; 

    [Header("Game End Thresholds")]
    public int winnerThreshold = 200;    

    
    public UnityEvent<int> onBalanceChanged;  
    public UnityEvent<int> onWin;              
    public UnityEvent onLose;
    public UnityEvent onSpinStarted;
    public UnityEvent onSpinFinished;
    public UnityEvent onGameOver;             // fires when balance hits 0
    public UnityEvent onWinner;               // fires when balance exceeds winnerThreshold

    private bool isSpinning = false;

    private void Start()
    {
        onBalanceChanged?.Invoke(playerBalance);
    }

  
    public void StartSpin()
    {
        if (isSpinning) return;

        if (playerBalance < betAmount)
        {
            Debug.Log("Not enough balance!");
            return;
        }

       
        playerBalance -= betAmount;
        onBalanceChanged?.Invoke(playerBalance);
        onSpinStarted?.Invoke();

   
        if (handleAnimator != null)
            handleAnimator.TriggerPull();

        StartCoroutine(SpinAllReels());
    }

    private IEnumerator SpinAllReels()
    {
        isSpinning = true;

       
        Coroutine[] spinCoroutines = new Coroutine[reels.Length];
        for (int i = 0; i < reels.Length; i++)
        {
            float delay = i * reelStaggerDelay;
            spinCoroutines[i] = StartCoroutine(reels[i].Spin(delay));
        }

        float totalWait = (reels.Length - 1) * reelStaggerDelay + reels[reels.Length - 1].spinDuration;
        yield return new WaitForSeconds(totalWait + 0.1f);

        isSpinning = false;
        onSpinFinished?.Invoke();
        CheckWin();
    }

    private void CheckWin()
    {
        SlotSymbol firstSymbol = reels[0].GetCurrentSymbol();
        bool isWin = true;

        foreach (Reel reel in reels)
        {
            if (reel.GetCurrentSymbol().symbolName != firstSymbol.symbolName)
            {
                isWin = false;
                break;
            }
        }

        if (isWin)
        {
            int payout = firstSymbol.payoutValue;
            playerBalance += payout;
            onBalanceChanged?.Invoke(playerBalance);
            onWin?.Invoke(payout);
            Debug.Log($"WIN! Payout: {payout}");
        }
        else
        {
            onLose?.Invoke();
            Debug.Log("No match. Try again!");
        }

       
        if (playerBalance >= winnerThreshold)
        {
            onWinner?.Invoke();
            StartCoroutine(ResetAfterDelay(3f));
        }
        else if (playerBalance <= 0)
        {
            onGameOver?.Invoke();
            StartCoroutine(ResetAfterDelay(3f));
        }
    }

      private IEnumerator ResetAfterDelay(float delay)
    {
      
        isSpinning = true;
        yield return new WaitForSeconds(delay);

        playerBalance = 100;
        isSpinning = false;
        onBalanceChanged?.Invoke(playerBalance);
        onSpinFinished?.Invoke(); 
    }
}
