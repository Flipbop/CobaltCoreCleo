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
		while (index >= 0 && Amount > 0)
		{
			if (c.hand[index].upgrade == Upgrade.None)
			{
				if (!c.hand[index].GetImpaired())
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedBTrait, true, false);
					ImprovedBExt.AddImprovedB(c.hand[index], s);
					Amount--;
					Audio.Play(Event.CardHandling);
				}
				else
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImpairedTrait, false, false);
					ImpairedExt.RemoveImpaired(c.hand[index], s, true);
					Amount--;
					Audio.Play(Event.CardHandling);
				}
			}
			index--;
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImproveBIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::ImproveB")
			{
				Icon = ModEntry.Instance.ImproveBIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImproveB", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImproveB", "description"])
			}
		];
}
