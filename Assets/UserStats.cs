using UnityEngine;

public class UserStats : MonoBehaviour
{
    public static UserStats Instance { get; private set; }

    public int coins = 0;
    public int highScore = 0;
    public int currentScore = 0;
    // must fetch from supabase postgres through the custom fast api backend

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on load
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
}
