using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImproveB : DynamicWidthCardAction
{
	public required int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		while (Amount > 0)
		{
			if (index < 0)
			{
				break;
			}
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
		=> new(ModEntry.Instance.ImproveAIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::ImproveB")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImproveB", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImproveB", "description"])
			}
		];
}
