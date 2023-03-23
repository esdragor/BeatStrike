
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(order = 0, menuName = "Pattern Interface Data",fileName = "PatternInterfaceData")]
public class PatternToolsInterfaceData : ScriptableObject
{
    public float leftWidth;
    public Rect previewRect;
    public Color previewBackgroundColor;
    public Rect patternDropdownRect;
    public Color patternDropdownBackgroundColor;
    public Rect timelineRectContent;
    public Rect selectedInteractionRect;
    public Color selectedInteractionBackgroundColor;
    public Rect patternDataRect;
    public Color patternDataBackgroundColor;
    public Rect interactionListRect;
    public Color interactionListBackgroundDropdown;

    [Header("Key")] 
    public float lineThickness;
    public float interactionIconWidth = 30f;
    public float interactionIconHeight = 30f;
    public Texture blueTapTexture;
    public Texture redTapTexture;
    public Color tapLine;
    public Texture slideTexture;
    public Color slideLine;

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
