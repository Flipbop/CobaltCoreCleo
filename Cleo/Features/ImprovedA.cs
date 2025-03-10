using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImprovedAExt
{
	public static int GetImprovedA(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(self, "ImprovedA");

	public static void SetImprovedA(this Card self, int value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedA", value);

	public static void AddImprovedA(this Card self, int value)
	{
		if (value != 0)
			self.SetImprovedA(self.GetImprovedA() + value);
	}
}

internal sealed class ImprovedAManager
{
	internal static ICardTraitEntry Trait = null!;

	public ImprovedAManager()
	{
		Trait = ModEntry.Instance.Helper.Content.Cards.RegisterTrait("ImprovedA", new()
		{
			Icon = (_, _) => ModEntry.Instance.ImprovedIcon.Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["cardTrait", "ImprovedA", "name"]).Localize,
			Tooltips = (_, card) => [ModEntry.Instance.Api.GetImprovedATooltip(card?.GetImprovedA() ?? 1)]
		});

		ModEntry.Instance.Helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, e) =>
		{
			if (e.Card.GetImprovedA() != 0)
				e.SetOverride(Trait, true);
		};

		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.ModifyBaseDamage), (Card? card, State state) =>
		{
			if (card is null)
				return 0;

			var strengthen = card.GetImprovedA();
			if (strengthen > 0 && state.EnumerateAllArtifacts().Any(a => a is CleoPeriArtifact))
				strengthen++;
			return strengthen;
		});

		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImprovedA() == 0)
					continue;
				card.SetImprovedA(0);
			}
		});
	}
}
