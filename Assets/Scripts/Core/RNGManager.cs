using UnityEngine;


public class RNGManager : MonoBehaviour
{
    /// Returns a random index between 0 (inclusive) and max (exclusive).
    public int GetRandomIndex(int max)
    {
        return Random.Range(0, max);
    }
}
