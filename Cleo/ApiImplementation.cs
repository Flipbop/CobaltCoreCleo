using Nickel;

namespace Flipbop.Cleo;

public sealed class ApiImplementation : ICleoApi
{
	public IDeckEntry CleoDeck
		=> ModEntry.Instance.CleoDeck;

	public IStatusEntry CrunchTimeStatus
		=> ModEntry.Instance.CrunchTimeStatus;

	public ICardTraitEntry ImprovedACardTrait
		=> ImprovedAManager.Trait;
	public ICardTraitEntry ImprovedBCardTrait
		=> ImprovedBManager.Trait;

	public Tooltip GetImprovedATooltip(int amount)
		=> new GlossaryTooltip($"cardtrait.{ModEntry.Instance.Package.Manifest.UniqueName}::ImprovedA")
		{
			Icon = ModEntry.Instance.ImprovedIcon.Sprite,
			TitleColor = Colors.cardtrait,
			Title = ModEntry.Instance.Localizations.Localize(["cardTrait", "Improved", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["cardTrait", "Improved", "description"], new { Damage = amount })
		};
	
	public Tooltip GetImprovedBTooltip(int amount)
		=> new GlossaryTooltip($"cardtrait.{ModEntry.Instance.Package.Manifest.UniqueName}::ImprovedB")
		{
			Icon = ModEntry.Instance.ImprovedIcon.Sprite,
			TitleColor = Colors.cardtrait,
			Title = ModEntry.Instance.Localizations.Localize(["cardTrait", "Improved", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["cardTrait", "Improved", "description"], new { Damage = amount })
		};
	
	public int GetImprovedA(Card card)
		=> card.GetImprovedA();

	public void SetImprovedA(Card card, int value)
		=> card.SetImprovedA(value);

	public void AddImprovedA(Card card, int value)
		=> card.AddImprovedA(value);
	
	public int GetImprovedB(Card card)
		=> card.GetImprovedB();

	public void SetImprovedB(Card card, int value)
		=> card.SetImprovedB(value);

	public void AddImprovedB(Card card, int value)
		=> card.AddImprovedB(value);

	public CardAction MakeImprovedAAction(int cardId, int amount)
		=> new AStrengthen { CardId = cardId, Amount = amount };

	public CardAction MakeImprovedBAction(int amount)
		=> new AStrengthenHand { Amount = amount };
}
