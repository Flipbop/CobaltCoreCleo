using Microsoft.Extensions.Logging;
using Nickel;
using Shockah.Kokoro;

namespace Flipbop.Cleo;

internal static class ImprovedAExt
{
	public static bool GetImprovedA(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "ImprovedA");

	public static void SetImprovedA(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedA", value);

	public static void AddImprovedA(this Card self,  State s)
	{
		if (self.GetImpaired())
		{
			self.RemoveImpaired(s, false);
		} else if (!self.GetImprovedA() && !self.GetImprovedB() && self.upgrade != Upgrade.A && self.upgrade != Upgrade.B)
		{
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.A);
		}
	}
	public static void RemoveImprovedA(this Card self, State s)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
		ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, self, ModEntry.Instance.ImprovedATrait, false, false);
	}
}

internal sealed class ImprovedAManager
{
	internal static readonly ICardTraitEntry Trait = ModEntry.Instance.ImprovedATrait;

	public ImprovedAManager()
	{
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (State state, Card card) =>
		{
			if (ModEntry.Instance.Helper.Content.Cards.IsCardTraitActive(state, card, Trait))
			{
				card.RemoveImprovedA(state);
			}
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImprovedA())
				{
					card.RemoveImprovedA(state);
				}
			}
		});
	}
}
