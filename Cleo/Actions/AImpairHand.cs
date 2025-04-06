using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImpairHand : DynamicWidthCardAction
{
	public int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		while (index >= 0)
		{
			if (c.hand[index].upgrade != Upgrade.None)
			{
				if (!c.hand[index].GetImprovedA() && !c.hand[index].GetImprovedB())
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImpairedTrait, true, false);
					ImpairedExt.AddImpaired(c.hand[index], s);
					Amount--;
					Audio.Play(Event.CardHandling);
				}
				else
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedATrait, false, false);
					ImprovedAExt.RemoveImprovedA(c.hand[index], s);
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedBTrait, false, false);
					ImprovedBExt.RemoveImprovedB(c.hand[index], s);
					Amount--;
					Audio.Play(Event.CardHandling);
				}
			} 
			index--;
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairHandIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Impair Hand")
			{
				Icon = ModEntry.Instance.ImpairHandIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImpairHand", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImpairHand", "description"])
			}
		];
}
