using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class SwapNotesCard : Card, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.CleoDeck.Deck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/ProfitMargin.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SwapNotes", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 0,
			infinite = upgrade == Upgrade.B,
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.A => [
				new AImpair { Amount = 1 },
				new AShuffleHand(),
				new AImproveA { Amount = 3 },
			],
			Upgrade.B => [
				new AImpair { Amount = 1 },
				new AShuffleHand(),
				new AImproveB { Amount = 1 },
			],
			_ => [
				new AImpair { Amount = 1 },
				new AShuffleHand(),
				new AImproveA { Amount = 2 },
			]
		};
}
