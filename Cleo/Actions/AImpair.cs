﻿using FSPRO;
using Nickel;
using System.Collections.Generic;
using System.Linq;


namespace Flipbop.Cleo;

public sealed class AImpair : DynamicWidthCardAction
{
	public required int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		while (index >= 0 && Amount > 0)
		{
			if (c.hand[index].upgrade != Upgrade.None)
			{
				if (!c.hand[index].GetIsImprovedA() && !c.hand[index].GetIsImprovedB())
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImpairedTrait, true, false);
					ImpairedExt.AddImpaired(c.hand[index], s);
				}
				else
				{
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedATrait, false, false);
					ImprovedAExt.RemoveImprovedA(c.hand[index], s);
					ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedBTrait, false, false);
					ImprovedBExt.RemoveImprovedB(c.hand[index], s);
				}
				Amount--;
				Audio.Play(Event.CardHandling);
				if (s.EnumerateAllArtifacts().Any((a) => a is CleoDrakeArtifact))
				{
					c.Queue(new AStatus { targetPlayer = true, status = Status.heat, statusAmount = 1 });
				}
				if (s.EnumerateAllArtifacts().Any((a) => a is CleoDizzyArtifact))
				{
					c.Queue(new AStatus { targetPlayer = true, status = Status.tempShield, statusAmount = 1 });
					if (c.hand[index].GetMeta().deck == Deck.dizzy)
					{
						c.Queue(new AImproveASelf() { id = c.hand[index].uuid });
					}
				}
			}
			index--;
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
			}
		];
}
