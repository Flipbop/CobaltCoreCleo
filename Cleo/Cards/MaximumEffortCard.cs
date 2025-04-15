using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class MaximumEffortCard : Card, IRegisterable
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
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "MaximumEffort", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 2,
			infinite = upgrade == Upgrade.B,
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.A => [
				new AAttack{ damage = GetDmg(s, 2)},
				new AImpairToAction { Amount = 1, action = new AAttack{ damage = GetDmg(s, 2)} },
				new AImpairToAction { Amount = 1, action = new AAttack{ damage = GetDmg(s, 2)} },
			],
			Upgrade.B => [
				new AImpairToAction { Amount = 2, action = new AAttack{ damage = GetDmg(s, 3)} },
				new AImpairToAction { Amount = 1, action = new AAttack{ damage = GetDmg(s, 2)} },
				new AImpairToAction { Amount = 2, action = new AAttack{ damage = GetDmg(s, 4)} },
			],
			_ => [
				new AImpairToAction { Amount = 1, action = new AAttack{ damage = GetDmg(s, 2)} },
				new AImpairToAction { Amount = 1, action = new AAttack{ damage = GetDmg(s, 2)} },
				new AImpairToAction { Amount = 1, action = new AAttack{ damage = GetDmg(s, 2)} },
			]
		};
}
