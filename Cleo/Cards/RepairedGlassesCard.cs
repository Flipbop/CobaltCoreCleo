using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Flipbop.Cleo;

internal sealed class RepairedGlassesCard : Card, IRegisterable
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
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/colorless.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RepairedGlasses", "name"]).Localize
		});
	}

	

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 2,
			exhaust = true,
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.A =>
			[
				new ImprovedCannonCard.AUpgradeHint(),
				new AStatus { targetPlayer = true, status = Status.energyNextTurn, statusAmount = c.hand.Count(card => card.upgrade != Upgrade.None), xHint = 1},
				new AStatus { targetPlayer = true, status = Status.drawNextTurn, statusAmount = 2}
			],
			Upgrade.B => [
				new ADiscard {count = 2},
				new ImprovedCannonCard.AUpgradeDiscardHint(),
				new AStatus { targetPlayer = true, status = Status.energyNextTurn, statusAmount = c.discard.Count(card => card.upgrade != Upgrade.None), xHint = 1},
			],
			_ => [
				new ImprovedCannonCard.AUpgradeHint(),
				new AStatus { targetPlayer = true, status = Status.energyNextTurn, statusAmount = c.hand.Count(card => card.upgrade != Upgrade.None), xHint = 1},
			]
			
		};
	
}
