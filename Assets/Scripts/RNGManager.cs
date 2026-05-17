using UnityEngine;

public class RNGManager : MonoBehaviour
{
    public int GetRandomIndex(int max)
    {
        return Random.Range(0, max);
    }
}