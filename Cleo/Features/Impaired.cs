using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImpairedExt
{
	private static Upgrade upgradeContainer = Upgrade.None;
	public static bool GetImpaired(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "Impaired");

	public static void SetImpaired(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "Impaired", value);

	public static void AddImpaired(this Card self, State s)
	{
		if (self.GetImprovedA())
		{
			self.RemoveImprovedA(s);
		} else if (self.GetImprovedB())
		{
			self.RemoveImprovedB(s);
		} else if (!self.GetImpaired() && self.upgrade != Upgrade.None)
		{
			upgradeContainer = self.upgrade;
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.None);
			
		}
	}

	public static void RemoveImpaired(this Card self, State s)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetPermanentUpgrade(self, upgradeContainer);
		ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, self, ModEntry.Instance.ImpairedTrait, false, false);

	}
}

internal sealed class ImpairedManager
{
	internal static ICardTraitEntry Trait = null!;
	
	public ImpairedManager()
	{
		Trait = ModEntry.Instance.ImpairedTrait;
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (State state, Card card) =>
		{
			if (ModEntry.Instance.Helper.Content.Cards.IsCardTraitActive(state, card, Trait))
			{
				card.RemoveImpaired(state);
			}
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImpaired())
				{
					card.RemoveImpaired(state);
				}
			}
		});
	}
}
