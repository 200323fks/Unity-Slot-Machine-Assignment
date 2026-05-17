using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Reel[] reels;
    public int playerBalance = 100;

    public void StartSpin()
    {
        StartCoroutine(SpinReels());
    }

    IEnumerator SpinReels()
    {
        foreach (Reel reel in reels)
        {
            yield return StartCoroutine(reel.Spin());
        }

        CheckWin();
    }

    void CheckWin()
    {
        SlotSymbol first = reels[0].GetCurrentSymbol();
        bool isWin = true;

        foreach (Reel reel in reels)
        {
            if (reel.GetCurrentSymbol().symbolName != first.symbolName)
            {
                isWin = false;
                break;
            }
        }

        if (isWin)
        {
            playerBalance += first.payoutValue;
            Debug.Log("You Win!");
        }
        else
        {
            playerBalance -= 10;
            Debug.Log("You Lose!");
        }
    }
}