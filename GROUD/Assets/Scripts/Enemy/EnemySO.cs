using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Enemy/Data", fileName = "EnemyData")]
public class EnemySO : ScriptableObject
{
    public float healthPoint;
    public GameObject visual;
    public Pattern patternSO;
}
