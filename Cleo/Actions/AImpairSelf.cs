using FSPRO;
using Nickel;
using System.Collections.Generic;
using System.Linq;


namespace Flipbop.Cleo;

public sealed class AImpairSelf : DynamicWidthCardAction
{
	public required int id;


	public override void Begin(G g, State s, Combat c)
	{
		if (s.FindCard(id) is Card card)
		{
			base.Begin(g, s, c);
			if (s.FindCard(id)!.GetImprovedA() || s.FindCard(id)!.GetImprovedB())
			{
				ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImprovedATrait, false, false);
				ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImprovedBTrait, false, false);
				ImprovedAExt.RemoveImprovedA(s.FindCard(id)!, s);
				ImprovedBExt.RemoveImprovedB(s.FindCard(id)!, s);
				Audio.Play(Event.CardHandling);
			}
			else if (s.FindCard(id)!.upgrade != Upgrade.None)
			{
				ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImpairedTrait, true,
					false);
				ImpairedExt.AddImpaired(card, s);
				Audio.Play(Event.CardHandling);
			}
			if (s.EnumerateAllArtifacts().Any((a) => a is CleoDrakeArtifact))
			{
				c.Queue(new AStatus { targetPlayer = true, status = Status.heat, statusAmount = 1 });
			}
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairedIcon.Sprite, null, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Self Impair")
			{
				Icon = ModEntry.Instance.ImpairedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "SelfImpair", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "SelfImpair", "description"])
			}
		];
}
