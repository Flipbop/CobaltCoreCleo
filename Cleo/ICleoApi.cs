using Nickel;

namespace Flipbop.Cleo;

public interface ICleoApi
{
	IDeckEntry CleoDeck { get; }
	IStatusEntry CrunchTimeStatus { get; }

	ICardTraitEntry ImprovedACardTrait { get; }
	ICardTraitEntry ImprovedBCardTrait { get; }
	Tooltip GetImprovedATooltip(bool onOrOff);
	Tooltip GetImprovedBTooltip(bool onOrOff);
	Tooltip GetImpairedTooltip(bool onOrOff);
	bool GetImprovedA(Card card);
	void SetImprovedA(Card card, bool value);
	void AddImprovedA(Card card, bool value);
	bool GetImprovedB(Card card);
	void SetImprovedB(Card card, bool value);
	void AddImprovedB(Card card, bool value);
	bool GetImpaired(Card card);
	void SetImpaired(Card card, bool value);
	void AddImpaired(Card card, bool value);
}
