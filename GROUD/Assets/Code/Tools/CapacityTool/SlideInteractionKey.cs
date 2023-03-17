using Utilities;

public class SlideInteractionKey : InteractionKey
{
    public float duration = 0.2f;
    public float outputTime;

    public SlideInteractionKey(int row, float time, string timeCode, Enums.InteractionType interactionType) : base(row, time, timeCode, interactionType)
    {
        this.row = row;
        this.interactionType = interactionType;
        this.timeCode = timeCode;
        this.time = time;
    }
}
