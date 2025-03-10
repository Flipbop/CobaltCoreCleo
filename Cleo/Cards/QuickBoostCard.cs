using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class QuickBoostCard : Card, IRegisterable
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
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/BuyLow.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "QuickBoost", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 0,
			description = ModEntry.Instance.Localizations.Localize(["card", "QuickBoost", "description", upgrade.ToString()])
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> [
			new AAddCard
			{
				destination = upgrade == Upgrade.B ? CardDestination.Deck : CardDestination.Hand,
				card = new SmallRepairsCard
				{
					discount = upgrade == Upgrade.B ? -1 : 0
				},
				amount = upgrade == Upgrade.A ? 3 : 2
			}
		];
}
