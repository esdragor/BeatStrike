using UnityEngine;
using UnityEngine.UI;

public class UI_RandomBPMSelector : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    public void ShowButtons()
    {
        GameManager.instance.bpmIsRandoming = true;
        easyButton.gameObject.SetActive(true);
        mediumButton.gameObject.SetActive(true);
        hardButton.gameObject.SetActive(true);
    }

    public void HideButtons(byte index)
    {
        GameManager.instance.SetRandomBPM(GameManager.instance.GetBPMList(index));
        ScoreManager.ModifierDifficulty(index);
        
        easyButton.gameObject.SetActive(false);
        mediumButton.gameObject.SetActive(false);
        hardButton.gameObject.SetActive(false);
    }

    private void Awake()
    {
        easyButton.onClick.AddListener(() => HideButtons(0));
        mediumButton.onClick.AddListener(() => HideButtons(1));
        hardButton.onClick.AddListener(() => HideButtons(2));
    }
}