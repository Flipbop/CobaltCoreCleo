using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class SlipShotCard : Card, IRegisterable
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
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Layout.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SlipShot", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = upgrade == Upgrade.A ? 0 : 1,
			exhaust = true,
			description = ModEntry.Instance.Localizations.Localize(["card", "SlipShot", "description", upgrade.ToString()])
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.B => [
				new AAddCard
				{
					card = new BulletPointCard(),
					destination = CardDestination.Discard,
				},
				new AAddCard
				{
					card = new SlideTransitionCard(),
					destination = CardDestination.Discard,
				},
				new ADummyAction { dialogueSelector = $".Played::{ModEntry.Instance.Package.Manifest.UniqueName}::LayoutOrStrategize" },
			],
			_ => [
				new ASpecificCardOffering
				{
					Destination = CardDestination.Deck,
					Cards = [
						new BulletPointCard(),
						new SlideTransitionCard(),
					]
				},
				new ATooltipAction
				{
					Tooltips = [
						new TTCard { card = new BulletPointCard() },
						new TTCard { card = new SlideTransitionCard() },
					],
					dialogueSelector = $".Played::{ModEntry.Instance.Package.Manifest.UniqueName}::LayoutOrStrategize"
				},
			]
		};
}
