using Nickel;

namespace Flipbop.Cleo;

public interface ICleoApi
{
	IDeckEntry CleoDeck { get; }
	IStatusEntry CrunchTimeStatus { get; }

	ICardTraitEntry ImprovedACardTrait { get; }
	ICardTraitEntry ImprovedBCardTrait { get; }
	Tooltip GetImprovedATooltip(int amount);
	Tooltip GetImprovedBTooltip(int amount);
	int GetImprovedA(Card card);
	void SetImprovedA(Card card, int value);
	void AddImprovedA(Card card, int value);
	int GetImprovedB(Card card);
	void SetImprovedB(Card card, int value);
	void AddImprovedB(Card card, int value);
	CardAction MakeImprovedAAction(int cardId, int amount);
	CardAction MakeImprovedBAction(int amount);
}
