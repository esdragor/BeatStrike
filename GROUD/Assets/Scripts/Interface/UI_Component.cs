using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UI_Component : MonoBehaviour
{
    [Header("Animation")]
    public bool jumpWithBPM;
    public float jumpStrength = 0.1f;
    public int jumpVibrato = 3;
    
    private bool isJumping = false;
    private void Awake()
    {
        isJumping = false;
        if (jumpWithBPM)
        {
            GameManager.OnTick += JumpAnimation;
        }
    }

    private async void JumpAnimation()
    {
        if (isJumping) return;
        isJumping = true;
        Vector3 jumpVector = new Vector3(jumpStrength, jumpStrength, jumpStrength);
        transform.DOPunchScale(jumpVector, 1, jumpVibrato);
        await Task.Delay(1000);
        isJumping = false;
    }
}
