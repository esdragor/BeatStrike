using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UI_Announcer announcer;
    public UI_EndLevel endLevel;
    public UI_Score score;
    
    private void Awake()
    {
        instance = this;
    }
}
