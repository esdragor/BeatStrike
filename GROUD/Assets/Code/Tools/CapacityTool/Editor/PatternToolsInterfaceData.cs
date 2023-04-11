
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(order = 0, menuName = "Pattern Interface Data",fileName = "PatternInterfaceData")]
public class PatternToolsInterfaceData : ScriptableObject
{
    [Header("Contents")]
    public float leftWidth;
    public Rect previewRect;
    public Rect symphonyDropdownRect;
    public Rect timelineRectContent;
    public Rect selectedInteractionRect;
    public Rect patternDataRect;
    public Rect interactionListRect;

    public Rect symphonyCreatorPopupRect;
    
    public Rect explorationMelodyContent;
    public Rect enemyMelodyContent;
    public Rect bossMelodyContent;

    public Color contentBackgroundColor;
    
    [Header("Key")] 
    public float lineThickness;
    public float interactionIconWidth = 30f;
    public float interactionIconHeight = 30f;
    public Texture blueTapTexture;
    public Texture redTapTexture;
    public Color tapLine;
    public Texture swipeTexture;
    public Color swipeLine;
    public float swipeLineThickness;

    [Header("Others")] 
    public float buttonIconHeight;
    public float buttonIconWidth;
    public Texture saveTexture;
    public Texture deleteTexture;
    public float interactHandlerWidth;
    public float interactionHandlerHeight;
    public Texture endHandlerTexture;
    public float endLineThickness = 2f;
    public Color endLine;

}
