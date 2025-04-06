using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImpairedExt
{
	private static Upgrade _upgradeContainer = Upgrade.None;
	public static bool GetImpaired(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "Impaired");

	public static void SetImpaired(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "Impaired", value);

	public static void AddImpaired(this Card self, State s)
	{
		if (!self.GetImpaired() && self.upgrade != Upgrade.None && self.IsUpgradable())
		{
			_upgradeContainer = self.upgrade;
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.None);
		}
	}

	public static void RemoveImpaired(this Card self, State s, bool useStorage)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
		if (useStorage)
		{
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetPermanentUpgrade(self, _upgradeContainer);
		}
		ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, self, ModEntry.Instance.ImpairedTrait, false, false);
	}
}

internal sealed class ImpairedManager
{
	internal static readonly ICardTraitEntry Trait = ModEntry.Instance.ImpairedTrait;
	
	public ImpairedManager()
	{
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (State state, Card card) =>
		{
			if (ModEntry.Instance.Helper.Content.Cards.IsCardTraitActive(state, card, Trait))
			{
				card.RemoveImpaired(state, true);
			}
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImpaired())
				{
					card.RemoveImpaired(state, true);
				}
			}
		});
	}
}
