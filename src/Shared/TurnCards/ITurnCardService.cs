namespace Squads.Shared.TurnCards;

public interface ITurnCardService
{
    TurnCardResponse.GetIndex GetList(TurnCardRequest.GetIndex request);
    TurnCardResponse.GetDetail Get(TurnCardRequest.GetDetail request);
    TurnCardResponse.GetDetail Create(TurnCardRequest.Create request);
    TurnCardResponse.GetDetail Edit(TurnCardRequest.Edit request);
    TurnCardResponse.GetDetail ConsumeTurn(int turnCardId);
    void Delete(TurnCardRequest.Delete request);
}
