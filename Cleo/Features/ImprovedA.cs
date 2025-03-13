using Nickel;
using Shockah.Kokoro;

namespace Flipbop.Cleo;

internal static class ImprovedAExt
{
	public static bool GetImprovedA(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "ImprovedA");

	public static void SetImprovedA(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "ImprovedA", value);

	public static void AddImprovedA(this Card self)
	{
		if (self.GetImpaired())
		{
			self.RemoveImpaired();
		}else if (!self.GetImprovedA() && !self.GetImprovedB())
		{
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, Upgrade.A);
		}
	}
	public static void RemoveImprovedA(this Card self)
	{
		ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(self, null);
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
				if (card.GetImprovedA())
				{
					card.SetImprovedA(false);
				}
			}
		});
	}
}
