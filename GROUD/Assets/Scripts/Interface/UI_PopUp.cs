using DG.Tweening;
using UnityEngine;

public class UI_PopUp : MonoBehaviour
{
    public float toggleScale = 1f;
    public float duration = 1f;

    private void Awake()
    {
        TogglePopUp(false);
    }

    public void TogglePopUp(bool toggle)
    {
        if (toggle)
        {
            gameObject.SetActive(true);
            transform.DOScale(toggleScale, duration);
        }
        else
        {
            transform.DOScale(0, duration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
