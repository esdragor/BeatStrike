using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : MonoBehaviour
{
    public PopUpElement[] elements;
    private int index;

    public bool ended;
    
    private TMP_Text title => transform.GetChild(0).GetComponent<TMP_Text>();
    private Image illustration => transform.GetChild(1).GetComponent<Image>();
    private TMP_Text description => transform.GetChild(2).GetComponent<TMP_Text>();

    public void DrawNext()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            
            title.text = elements[index].title;
            illustration.sprite = elements[index].illustration;
            description.text = elements[index].description;
        }
        else
        {
            if (index < elements.Length - 1)
            {
            
                index++;
        
                title.text = elements[index].title;
                illustration.sprite = elements[index].illustration;
                description.text = elements[index].description;
            }
            else
            {
                ended = true;
                gameObject.SetActive(false);
            }
        }
    }
}

[Serializable] public struct PopUpElement
{
    public string title;
    public Sprite illustration;
    [TextArea] public string description;
}
