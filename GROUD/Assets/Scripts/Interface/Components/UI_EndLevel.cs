using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_EndLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private Transform DroppedItemParent;
    
    RectTransform tr;

    public void DrawPanel()
    {
        gameObject.SetActive(true);
        textScore.text = $"Score: \n\n{ScoreManager.GetScore()}";
        tr = Inventory.DropInventory(Rarity.Common).GetComponent<RectTransform>();
        tr.SetParent(DroppedItemParent);
        tr.position = Vector3.zero;
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
        Restart();
    }

    private void OnDisable()
    {
        if (tr != null)
        {
            Destroy(tr.gameObject);
        }
    }
}