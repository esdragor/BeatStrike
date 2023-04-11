using UnityEngine;
using UnityEngine.Serialization;

public class SymphonySO : ScriptableObject
{
   public string sName;
    
    public MelodySO exploration;
    public MelodySO enemy;
    public MelodySO boss;

    public int bpm = 60;
}
