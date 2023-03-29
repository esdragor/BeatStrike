using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionStats : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private Button btn;
    
    private CharacterInfos playerStats;

    private void Start()
    {
        btn.onClick.AddListener(ShowStats);
    }

    private void ShowStats()
    {
        MainMenuManager.instance.PrintCharacterInfos(playerStats);
    }

    public void SetPlayerStats(CharacterInfos _CharacterInfos)
    {
        playerStats = new CharacterInfos();
        playerStats.playerStats = _CharacterInfos.playerStats;
        playerStats.playerSprite = _CharacterInfos.playerSprite;
        playerStats.PowerInfo = _CharacterInfos.PowerInfo;
        playerImage.sprite = playerStats.playerSprite;
        
    }
}
