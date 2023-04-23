using UnityEngine;

public class DetectorVisual : MonoBehaviour
{
    public ParticleSystem missVFX;
    public ParticleSystem okVFX;
    public ParticleSystem greatVFX;
    public ParticleSystem perfectVFX;
    public ParticleSystem attackVFX;
    public ParticleSystem dodgeVFX;
    public ParticleSystem rightBopVFX;
    public ParticleSystem leftBopVFX;

    private void Awake()
    {
        GameManager.instance.detectorVisual = this;
    }

    public void PlayVFX(string title)
    {
        switch (title)
        {
            case "Miss":
                missVFX.Play();
                break;
            
            case "Ok":
                okVFX.Play();
                break;
            
            case "Great":
                greatVFX.Play();
                break;
            
            case "Perfect":
                perfectVFX.Play();
                break;
            
            case "Attack":
                attackVFX.Play();
                break;
            
            case "Dodge":
                dodgeVFX.Play();
                break;
            
            case "RightBop":
                rightBopVFX.Play();
                break;
            
            case "LeftBop":
                leftBopVFX.Play();
                break;
        }
    }
}
