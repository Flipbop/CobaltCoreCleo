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

	public static void AddImpaired(this Card self)
	{
		if (self.GetImprovedA())
		{
			self.RemoveImprovedA();
		} else if (self.GetImprovedB())
		{
			self.RemoveImprovedB();
		} else if (!self.GetImpaired())
		{
			upgradeContainer = self.upgrade;
			ImpairedManager.UpgradeStorage(upgradeContainer);
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.None);
		}
	}

	public static void RemoveImpaired(this Card self)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
	}
}

internal sealed class ImpairedManager
{
	private static Upgrade upgrade = Upgrade.None;
	internal static ICardTraitEntry Trait = null!;

	public static void UpgradeStorage(Upgrade upgradeContainer)
	{
		upgrade = upgradeContainer;
	}
	public ImpairedManager()
	{
		Trait = ModEntry.Instance.Helper.Content.Cards.RegisterTrait("Impaired", new()
		{
			Icon = (_, _) => ModEntry.Instance.ImpairedIcon.Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["cardTrait", "Impaired", "name"]).Localize,
			Tooltips = (_, card) => [ModEntry.Instance.Api.GetImpairedTooltip(card?.GetImprovedA() ?? true)]
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (State state, Card card) =>
		{
			if (ModEntry.Instance.Helper.Content.Cards.IsCardTraitActive(state, card, Trait))
			{
				ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade( card, null);
			}
			
		});
		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImpaired())
				{
					card.SetImpaired(false);
				}
			}
		});
	}
}
