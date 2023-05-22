using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Enemy/Data", fileName = "EnemyData")]
public class EnemySO : ScriptableObject
{
    public GameObject visual;
    public Material[] material;
    [Header("Stats")]
    public float healthPoint;
    public float damage;
    public float statModificatorValuePercentage = 10f;
}
