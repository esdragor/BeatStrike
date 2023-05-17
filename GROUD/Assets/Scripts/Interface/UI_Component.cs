using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_Component : MonoBehaviour
{
    [Header("Animation")]
    public bool jumpWithBPM;
    public float jumpStrength = 0.1f;
    public int jumpVibrato = 3;
    
    private void Awake()
    {
        if (jumpWithBPM)
        {
            GameManager.OnTick += JumpAnimation;
        }
    }

    private void JumpAnimation()
    {
        Vector3 jumpVector = new Vector3(jumpStrength, jumpStrength, jumpStrength);
        transform.DOPunchScale(jumpVector, 1, jumpVibrato);
    }
}
