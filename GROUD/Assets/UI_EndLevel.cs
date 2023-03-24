using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EndLevel : MonoBehaviour
{
    public void DrawPanel()
    {
      gameObject.SetActive(true);  
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
