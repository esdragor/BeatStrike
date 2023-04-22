using TMPro;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    private TMP_Text text => GetComponent<TMP_Text>();
    
    public void SetScore(int value)
    {
        text.text = value.ToString();
    }
}
