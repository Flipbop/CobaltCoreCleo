using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AImpairSelf : DynamicWidthCardAction
{
	public required int id;


	public override void Begin(G g, State s, Combat c)
	{
		if (s.FindCard(id) is Card card)
		{
			base.Begin(g, s, c);
			ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.ImpairedTrait, true,
				false);
			ImpairedExt.AddImpaired(card, s);
			Audio.Play(Event.CardHandling);
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairedIcon.Sprite, null, Colors.textMain);

	public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Self Impair")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "SelfImpair", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "SelfImpair", "description"])
			}
		];
}
