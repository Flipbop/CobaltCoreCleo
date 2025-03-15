using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImprovedBExt
{
	public static bool GetImprovedB(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "ImprovedB");

	public static void SetImprovedB(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedB", value);

	public static void AddImprovedB(this Card self, State s)
	{
		if (self.GetImpaired())
		{
			self.RemoveImpaired(s);
		} else if (!self.GetImprovedA() && !self.GetImprovedB() && self.upgrade != Upgrade.A && self.upgrade != Upgrade.B)
		{
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.B);
		}
	}
	public static void RemoveImprovedB(this Card self, State s)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
		ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, self, ModEntry.Instance.ImprovedATrait, false, false);
	}
}

internal sealed class ImprovedBManager
{
	internal static readonly ICardTraitEntry Trait = ModEntry.Instance.ImprovedBTrait;

	public ImprovedBManager()
	{
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (State state, Card card) =>
		{
			if (ModEntry.Instance.Helper.Content.Cards.IsCardTraitActive(state, card, Trait))
			{
				card.RemoveImprovedB(state);
			}
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImprovedB())
				{
					card.RemoveImprovedB(state);
				}
			}
		});
	}
}
