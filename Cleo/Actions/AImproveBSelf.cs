using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImproveBSelf : DynamicWidthCardAction
{
	public required int id;

	public override void Begin(G g, State s, Combat c)
	{
		if (s.FindCard(id) is Card card)
		{
			base.Begin(g, s, c);
			ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImprovedBTrait, true, false);
			ImprovedBExt.AddImprovedB(card, s);
			Audio.Play(Event.CardHandling);
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImprovedIcon.Sprite, null, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Self Improve B")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "SelfImproveB", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "SelfImproveB", "description"])
			}
		];
}
