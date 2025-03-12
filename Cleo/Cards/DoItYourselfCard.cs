using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace Flipbop.Cleo;

internal sealed class DoItYourselfCard : Card, IRegisterable
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
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "DoItYourself", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 2,
			exhaust = upgrade != Upgrade.B,
			singleUse = upgrade == Upgrade.B,
			description = ModEntry.Instance.Localizations.Localize(["card", "DoItYourself", "description", upgrade.ToString()])
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.A =>
			[
				new AAddCard { amount = upgrade == Upgrade.B ? 2 : 1, card = new SmallRepairsCard { upgrade = Upgrade.A } },
				new AStatus { targetPlayer = true, status = Status.shield, statusAmount = 2 },
			],
			_ =>
			[
				new AAddCard { amount = upgrade == Upgrade.B ? 2 : 1, card = new SmallRepairsCard() },
				new AStatus { targetPlayer = true, status = Status.shield, statusAmount = 2 },
			]
		};
	
		
}
