using TMPro;
using UnityEngine;

public class UI_EndLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;

    public void DrawPanel()
    {
        gameObject.SetActive(true);
        textScore.text = $"Score: \n\n{ScoreManager.GetScore()}";
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        DisablePanel();
        GameLoopManager.instance.Restart();
    }

    public void Quit()
    {
       
        Application.Quit();
    }
}