using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImprovedAExt
{
	public static bool GetImprovedA(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "ImprovedA");

	public static void SetImprovedA(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedA", value);

	public static void AddImprovedA(this Card self)
	{
		if (!self.GetImprovedA() && !self.GetImprovedB())
		{
			self.upgrade = Upgrade.A;
		}
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
