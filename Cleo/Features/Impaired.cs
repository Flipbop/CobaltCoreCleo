using Nickel;
using System.Linq;

namespace Flipbop.Cleo;

internal static class ImpairedExt
{
	public static bool GetImpaired(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "Impaired");

	public static void SetImpaired(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "Impaired", value);

	public static void AddImpaired(this Card self, bool value)
	{
		if (!value)
			self.SetImpaired(value);
	}
}

internal sealed class ImpairedManager
{
	internal static ICardTraitEntry Trait = null!;

	public ImpairedManager()
	{
		Trait = ModEntry.Instance.Helper.Content.Cards.RegisterTrait("Impaired", new()
		{
			Icon = (_, _) => ModEntry.Instance.ImpairedIcon.Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["cardTrait", "Impaired", "name"]).Localize,
			Tooltips = (_, card) => [ModEntry.Instance.Api.GetImpairedTooltip(card?.GetImprovedA() ?? true)]
		});

		Upgrade upgrade = Upgrade.None;
		
		ModEntry.Instance.Helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, e) =>
		{
			if (!e.Card.GetImpaired())
			{
				e.SetOverride(Trait, true);
				upgrade = e.Card.upgrade;
				e.Card.upgrade = Upgrade.None;
			}
				
		};
		

		ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnCombatEnd), (State state) =>
		{
			foreach (var card in state.deck)
			{
				if (card.GetImpaired())
					continue;
				card.SetImpaired(false);
				card.upgrade = upgrade;
			}
		});
	}
}
