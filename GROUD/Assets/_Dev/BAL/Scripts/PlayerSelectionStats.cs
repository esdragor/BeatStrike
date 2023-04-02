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
        playerStats.power = new Power();
    }

    private void ShowStats()
    {
        MainMenuManager.instance.PrintCharacterInfos(playerStats);
    }

    public void SetPlayerStats(CharacterInfos _CharacterInfos)
    {
        playerStats = ScriptableObject.CreateInstance<CharacterInfos>();
        playerStats.playerStats = _CharacterInfos.playerStats;
        playerStats.playerSprite = _CharacterInfos.playerSprite;
        playerImage.sprite = playerStats.playerSprite;
        
    }
}