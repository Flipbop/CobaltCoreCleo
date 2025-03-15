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
	public static void RemoveImprovedB(this Card self)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
	}
}

internal sealed class ImprovedBManager
{
	internal static ICardTraitEntry Trait = null!;

	public ImprovedBManager()
	{
		Trait = ModEntry.Instance.ImprovedBTrait;
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
				if (card.GetImprovedB())
				{
					card.SetImprovedB(false);
				}
			}
		});
	}
}
