
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Pattern Interface Data",fileName = "PatternInterfaceData")]
public class PatternToolsInterfaceData : ScriptableObject
{
    public float leftWidth;
    public Rect previewRect;
    public Rect patternDropdownRect;
    public Rect timelineRectContent;
    public Rect selectedInteractionRect;
    public Rect patternDataRect;
    public Rect interactionListRect;
}
