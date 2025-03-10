using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImprovedBExt
{
	public static int GetImprovedB(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(self, "ImprovedA");

	public static void SetImprovedB(this Card self, int value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedA", value);

	public static void AddImprovedB(this Card self, int value)
	{
		if (value != 0)
			self.SetImprovedB(self.GetImprovedB() + value);
	}
}

internal sealed class ImprovedBManager
{
	internal static ICardTraitEntry Trait = null!;

	public ImprovedBManager()
	{
		Trait = ModEntry.Instance.Helper.Content.Cards.RegisterTrait("ImprovedB", new()
		{
			Icon = (_, _) => ModEntry.Instance.ImprovedIcon.Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["cardTrait", "ImprovedB", "name"]).Localize,
			Tooltips = (_, card) => [ModEntry.Instance.Api.GetImprovedATooltip(card?.GetImprovedA() ?? 1)]
		});

		ModEntry.Instance.Helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, e) =>
		{
			if (e.Card.GetImprovedB() != 0)
				e.SetOverride(Trait, true);
		};

		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.ModifyBaseDamage), (Card? card, State state) =>
		{
			if (card is null)
				return 0;

			var strengthen = card.GetImprovedB();
			if (strengthen > 0 && state.EnumerateAllArtifacts().Any(a => a is CleoPeriArtifact))
				strengthen++;
			return strengthen;
		});

		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImprovedB() == 0)
					continue;
				card.SetImprovedB(0);
			}
		});
	}
}
