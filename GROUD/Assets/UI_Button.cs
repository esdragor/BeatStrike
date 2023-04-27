using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Button : UI_Component
{
    private Button button => GetComponent<Button>();

    private void Start()
    {
        button.onClick.AddListener(AnimationAnimBehaviour);
    }


    private void AnimationAnimBehaviour()
    {
        //Create Animation Behaviours
    }
}
