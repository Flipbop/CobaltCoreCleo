using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImpairSelf : DynamicWidthCardAction
{

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		
		if (index < 0)
		{ 
			;
		}
		if (c.hand[index].upgrade != Upgrade.None)
		{
			c.hand[index].upgrade = Upgrade.None;
			Audio.Play(Event.CardHandling);
		}
		index--;
		
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImprovedIcon.Sprite, 1, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Impair")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "Impair", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "Impair", "description"])
			}
		];
}
