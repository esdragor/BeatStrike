using TMPro;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    private TMP_Text text => GetComponent<TMP_Text>();
    
    public void SetScore(float value)
    {
        text.text = $"DISTANCE : {value}";
    }
}
