using UnityEngine;

/// <summary>
/// Centralized RNG service. All randomization goes through here for fairness and testability.
/// </summary>
public class RNGManager : MonoBehaviour
{
    /// <summary>Returns a random index between 0 (inclusive) and max (exclusive).</summary>
    public int GetRandomIndex(int max)
    {
        return Random.Range(0, max);
    }
}
