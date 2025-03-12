using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImprovedAExt
{
	public static bool GetImprovedA(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "ImprovedA");

	public static void SetImprovedA(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedA", value);

	public static void AddImprovedA(this Card self, bool value)
	{
		if (!value)
			self.SetImprovedA(value);
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
			Tooltips = (_, card) => [ModEntry.Instance.Api.GetImprovedATooltip(card?.GetImprovedA() ?? true)]
		});

		ModEntry.Instance.Helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, e) =>
		{
			if (!e.Card.GetImprovedA() && !e.Card.GetImprovedB())
			{
				e.SetOverride(Trait, true);
				e.Card.upgrade = Upgrade.A;
			}
				
		};
		

		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImprovedA())
					continue;
				card.SetImprovedA(false);
				card.upgrade = Upgrade.None;
			}
		});
	}
}
