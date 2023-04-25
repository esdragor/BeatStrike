using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
        DisablePanel();
    }
}