using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        onUpdated?.Invoke();
    }
}
