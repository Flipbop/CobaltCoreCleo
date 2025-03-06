using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class SmallRepairsCard : Card, IRegisterable
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
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = true,
			},
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Special/Brainstorm.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SmallRepairs", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 1,
			temporary = true
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> [
			new ADrawCard
			{
				count = upgrade switch
				{
					Upgrade.A => 2,
					Upgrade.B => 3,
					_ => 1
				}
			},
			new AStatus
			{
				targetPlayer = true,
				status = Status.drawNextTurn,
				statusAmount = upgrade == Upgrade.A ? 4 : 3,
			}
		];
}
