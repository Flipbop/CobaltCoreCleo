using FSPRO;
using Nickel;
using System.Collections.Generic;

namespace Flipbop.Cleo;

public sealed class AStrengthenHand : DynamicWidthCardAction
{
	public required int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		foreach (var card in c.hand)
			card.AddImprovedA(Amount);
		Audio.Play(Event.Status_PowerUp);
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImprovedIcon.Sprite, Amount, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Improve A (FALSE)")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImproveA", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImproveA", "description"], new { Damage = Amount })
			}
		];
}
