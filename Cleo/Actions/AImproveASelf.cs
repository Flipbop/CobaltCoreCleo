using FSPRO;
using Nickel;
using System.Collections.Generic;
using System.Linq;


namespace Flipbop.Cleo;

public sealed class AImproveASelf : DynamicWidthCardAction
{
	public required int id;

	public override void Begin(G g, State s, Combat c)
	{
		if (s.FindCard(id) is Card card)
		{
			base.Begin(g, s, c);
			if (s.FindCard(id)!.GetImpaired())
			{
				ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImpairedTrait, false, false);
				ImpairedExt.RemoveImpaired(card, s, false);
				Audio.Play(Event.CardHandling);
			} else if (s.FindCard(id)!.upgrade == Upgrade.None)
			{
				ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImprovedATrait, true, false);
				ImprovedAExt.AddImprovedA(card, s);
				Audio.Play(Event.CardHandling);
			}
			if (s.EnumerateAllArtifacts().Any((a) => a is CleoDrakeArtifact))
			{
				c.Queue(new AStatus { targetPlayer = true, status = Status.heat, statusAmount = -1 });
			}
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImprovedIcon.Sprite, null, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Self Improve A")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "SelfImproveA", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "SelfImproveA", "description"])
			}
		];
}
