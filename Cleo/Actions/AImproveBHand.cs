using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImproveBHand : DynamicWidthCardAction
{
	public required int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		while (index >= 0)
		{
			if (c.hand[index].upgrade == Upgrade.None)
			{
				c.hand[index].upgrade = Upgrade.B;
				Amount--;
				Audio.Play(Event.CardHandling);
			}
			index--;
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImproveBHandIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Improve B Hand")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImproveBHand", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImproveBHand", "description"])
			}
		];
}
