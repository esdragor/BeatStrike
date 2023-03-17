using Utilities;

public class TapInteractionKey : InteractionKey
{
    public TapInteractionKey(int row, float time, string timeCode, Enums.InteractionType keyType) : base(row, time, timeCode, keyType)
    {
        this.row = row;
        this.interactionType = interactionType;
        this.timeCode = timeCode;
        this.time = time;
    }
}
