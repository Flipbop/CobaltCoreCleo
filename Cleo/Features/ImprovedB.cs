using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImprovedBExt
{
	public static bool GetImprovedB(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "ImprovedB");

	public static void SetImprovedB(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedB", value);

	public static void AddImprovedB(this Card self)
	{
		if (!self.GetImprovedA() && !self.GetImprovedB())
		{
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.B);
		}
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
			Tooltips = (_, card) => [ModEntry.Instance.Api.GetImprovedBTooltip(card?.GetImprovedB() ?? true)]
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImprovedB())
				{
					card.SetImprovedB(false);
				}
			}
		});
	}
}
