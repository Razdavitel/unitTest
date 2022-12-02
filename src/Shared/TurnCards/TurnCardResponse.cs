namespace Squads.Shared.TurnCards;

public static class TurnCardResponse
{
    public class GetIndex
    {
        public List<TurnCardDto.Index> TurnCards { get; set; } = new();
        public int TotalAmount { get; set; }
    }

    public class GetDetail
    {
        public TurnCardDto.Detail TurnCard { get; set; }
    }
}
