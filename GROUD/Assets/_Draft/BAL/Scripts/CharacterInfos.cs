using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfos", menuName = "CharacterInfos", order = 0)]
public class CharacterInfos : ScriptableObject
{
    public PlayerStats playerStats;
    public Sprite playerSprite;
    public Power power;
    public Gear[] equipment = { null, null, null };
    
    public void SetPlayerStats(CharacterInfos _CharacterInfos)
    {
        playerStats = new PlayerStats(_CharacterInfos.playerStats);
        playerSprite = _CharacterInfos.playerSprite;
        power = _CharacterInfos.power;
        equipment = new []{null, null, (Gear)null};
    }
}
