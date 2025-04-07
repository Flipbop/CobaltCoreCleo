using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class NecessarySacrificeCard : Card, IRegisterable
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
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "NecessarySacrifice", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 2,
			retain = upgrade == Upgrade.B,
			exhaust = true,
			description = ModEntry.Instance.Localizations.Localize(["card", "NecessarySacrifice", "description", upgrade.ToString()]),
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=>
		[
			new AStatus { targetPlayer = true, status = Status.shield, statusAmount = s.ship.GetMaxShield() },
			new AImpair { Amount = upgrade == Upgrade.A ? 1 : 2}
		];
}
