﻿using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImproveAHand : DynamicWidthCardAction
{
	public int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		while (index >= 0)
		{
			if (c.hand[index].upgrade == Upgrade.None)
			{
				if (!c.hand[index].GetImpaired() && c.hand[index].IsUpgradable() && c.hand[index].GetMeta().deck != Deck.trash)
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedATrait, true, false);
					ImprovedAExt.AddImprovedA(c.hand[index], s);
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
		=> new(ModEntry.Instance.ImproveAHandIcon.Sprite, null, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Improve A Hand")
			{
				Icon = ModEntry.Instance.ImproveAHandIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImproveAHand", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImproveAHand", "description"])
			}
		];
}
