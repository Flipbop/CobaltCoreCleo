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

	public Tooltip GetImprovedATooltip(bool amount)
		=> new GlossaryTooltip($"cardtrait.{ModEntry.Instance.Package.Manifest.UniqueName}::ImprovedA")
		{
			Icon = ModEntry.Instance.ImprovedIcon.Sprite,
			TitleColor = Colors.cardtrait,
			Title = ModEntry.Instance.Localizations.Localize(["cardTrait", "ImprovedA", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["cardTrait", "ImprovedA", "description"])
		};
	
	public Tooltip GetImprovedBTooltip(bool amount)
		=> new GlossaryTooltip($"cardtrait.{ModEntry.Instance.Package.Manifest.UniqueName}::ImprovedB")
		{
			Icon = ModEntry.Instance.ImprovedIcon.Sprite,
			TitleColor = Colors.cardtrait,
			Title = ModEntry.Instance.Localizations.Localize(["cardTrait", "ImprovedB", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["cardTrait", "ImprovedB", "description"])
		};
	public Tooltip GetImpairedTooltip(bool amount)
		=> new GlossaryTooltip($"cardtrait.{ModEntry.Instance.Package.Manifest.UniqueName}::Impaired")
		{
			Icon = ModEntry.Instance.ImprovedIcon.Sprite,
			TitleColor = Colors.cardtrait,
			Title = ModEntry.Instance.Localizations.Localize(["cardTrait", "Impaired", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["cardTrait", "Impaired", "description"])
		};
	
	public bool GetImprovedA(Card card)
		=> card.GetImprovedA();

	public void SetImprovedA(Card card, bool value)
		=> card.SetImprovedA(value);

	public void AddImprovedA(Card card)
		=> card.AddImprovedA();
	
	public bool GetImprovedB(Card card)
		=> card.GetImprovedB();

	public void SetImprovedB(Card card, bool value)
		=> card.SetImprovedB(value);

	public void AddImprovedB(Card card)
		=> card.AddImprovedB();
	public bool GetImpaired(Card card)
		=> card.GetImprovedB();

	public void SetImpaired(Card card, bool value)
		=> card.SetImprovedB(value);

	public void AddImpaired(Card card)
		=> card.AddImprovedB();

}
