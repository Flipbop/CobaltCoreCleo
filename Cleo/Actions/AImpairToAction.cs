using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImpairToAction : DynamicWidthCardAction
{
	public required int Amount;
	public required CardAction action;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		bool impairedSuccesfully = false;
		int upgradeCounter = 0;
		while (index >= 0 && Amount > 0)
		{
			if (c.hand[index].upgrade != Upgrade.None)
			{
				upgradeCounter++;
				Amount--;
				if (Amount == 0)
				{
					impairedSuccesfully = true;
				}
			}
			index--;
		}
		index = c.hand.Count - 1;
		while (index >= 0 && upgradeCounter > 0 && impairedSuccesfully)
		{
			if (c.hand[index].upgrade != Upgrade.None)
			{
				if (!c.hand[index].GetImprovedA() && !c.hand[index].GetImprovedB())
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImpairedTrait, true, false);
					ImpairedExt.AddImpaired(c.hand[index], s);
					upgradeCounter--;
					Audio.Play(Event.CardHandling);
				}
				else
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedATrait, false, false);
					ImprovedAExt.RemoveImprovedA(c.hand[index], s);
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedBTrait, false, false);
					ImprovedBExt.RemoveImprovedB(c.hand[index], s);
					upgradeCounter--;
					Audio.Play(Event.CardHandling);
				}
			}
			index--;
		}
		if (impairedSuccesfully)
		{
			c.Queue(action);
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairedIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);
	

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Impair")
			{
				Icon = ModEntry.Instance.ImpairedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "Impair", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "Impair", "description"])
			},
			
		];
}
