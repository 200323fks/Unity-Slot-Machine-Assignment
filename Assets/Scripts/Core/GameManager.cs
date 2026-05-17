using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Central game controller. Manages spin flow, balance, and win/lose logic.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Reels")]
    public Reel[] reels;              // Assign all 3 reel objects in Inspector

    [Header("Handle")]
    public HandleAnimator handleAnimator; // Assign Handle_Image's HandleAnimator here

    [Header("Balance Settings")]
    public int playerBalance = 100;
    public int betAmount = 10;        // Cost per spin

    [Header("Stagger Settings")]
    public float reelStaggerDelay = 0.2f; // Delay between each reel starting

    [Header("Game End Thresholds")]
    public int winnerThreshold = 200;     // Balance above this = WINNER

    // Events so UIManager can react without tight coupling
    public UnityEvent<int> onBalanceChanged;   // fires with new balance
    public UnityEvent<int> onWin;              // fires with payout amount
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

    /// <summary>
    /// Called by the Spin button. Deducts bet and starts all reels.
    /// </summary>
    public void StartSpin()
    {
        if (isSpinning) return;

        if (playerBalance < betAmount)
        {
            Debug.Log("Not enough balance!");
            return;
        }

        // Deduct bet before spinning
        playerBalance -= betAmount;
        onBalanceChanged?.Invoke(playerBalance);
        onSpinStarted?.Invoke();

        // Trigger handle pull animation
        if (handleAnimator != null)
            handleAnimator.TriggerPull();

        StartCoroutine(SpinAllReels());
    }

    private IEnumerator SpinAllReels()
    {
        isSpinning = true;

        // Launch all reels with staggered delays (cascading effect)
        Coroutine[] spinCoroutines = new Coroutine[reels.Length];
        for (int i = 0; i < reels.Length; i++)
        {
            float delay = i * reelStaggerDelay;
            spinCoroutines[i] = StartCoroutine(reels[i].Spin(delay));
        }

        // Wait for the last reel to finish
        // Last reel takes longest: its delay + spinDuration
        float totalWait = (reels.Length - 1) * reelStaggerDelay + reels[reels.Length - 1].spinDuration;
        yield return new WaitForSeconds(totalWait + 0.1f);

        isSpinning = false;
        onSpinFinished?.Invoke();
        CheckWin();
    }

    /// <summary>
    /// Checks if all reels show the same symbol. Awards payout if true.
    /// </summary>
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

        // Check game-ending conditions after every spin result
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

    /// <summary>Resets balance and UI back to starting state after a delay.</summary>
    private IEnumerator ResetAfterDelay(float delay)
    {
        // Lock the spin button during the end-game popup
        isSpinning = true;
        yield return new WaitForSeconds(delay);

        playerBalance = 100;
        isSpinning = false;
        onBalanceChanged?.Invoke(playerBalance);
        onSpinFinished?.Invoke(); // re-enables spin button via UIManager
    }
}
