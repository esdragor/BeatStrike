using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfos", menuName = "CharacterInfos", order = 0)]
public class CharacterInfos : ScriptableObject
{
    public PlayerStats playerStats;
    public Sprite playerSprite;
    public string PowerInfo;
}
