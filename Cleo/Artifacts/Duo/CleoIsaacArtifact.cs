using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class CleoIsaacArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;

		helper.Content.Artifacts.RegisterArtifact("CleoIsaac", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = api.DuoArtifactVanillaDeck,
				pools = [ArtifactPool.Common]
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/Duo/CleoIsaac.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Duo", "CleoIsaac", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Duo", "CleoIsaac", "description"]).Localize
		});

		api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.CleoDeck.Deck, Deck.goat]);

		SupplyCard.Register(package, helper);
		DemandCard.Register(package, helper);
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> [
			new TTCard { card = new SupplyCard() },
			new TTCard { card = new DemandCard() },
		];

	public override void OnTurnStart(State state, Combat combat)
	{
		base.OnTurnStart(state, combat);
		if (!combat.isPlayerTurn || combat.turn != 1)
			return;

		combat.Queue([
			new ADelay(),
			new ASpecificCardOffering
			{
				Destination = CardDestination.Deck,
				Cards = [
					new SupplyCard(),
					new DemandCard(),
				],
				artifactPulse = Key()
			}
		]);
	}

	private sealed class SupplyCard : Card, IRegisterable
	{
		public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
		{
			helper.Content.Cards.RegisterCard("CleoIsaacSupplyCard", new()
			{
				CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
				Meta = new()
				{
					deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
					rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
					upgradesTo = [Upgrade.A, Upgrade.B],
					dontOffer = true,
				},
				Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Duo/CleoIsaacSupply.png")).Sprite,
				Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Duo", "CleoIsaacSupply", "name"]).Localize
			});
		}

		public override CardData GetData(State state)
			=> new()
			{
				cost = upgrade == Upgrade.A ? 1 : 2,
				temporary = true
			};

		public override List<CardAction> GetActions(State s, Combat c)
			=> upgrade switch
			{
				Upgrade.B => [
					new ASpawn
					{
						thing = new ShieldDrone
						{
							targetPlayer = true,
						},
						offset = -1
					},
					new ASpawn
					{
						thing = new ShieldDrone
						{
							targetPlayer = true,
						}
					},
					new ASpawn
					{
						thing = new ShieldDrone
						{
							targetPlayer = true,
						},
						offset = 1
					}
				],
				_ => [
					new ASpawn
					{
						thing = new ShieldDrone
						{
							targetPlayer = true,
						},
						offset = -1
					},
					new ASpawn
					{
						thing = new ShieldDrone
						{
							targetPlayer = true,
						},
						offset = 1
					}
				]
			};
	}

	private sealed class DemandCard : Card, IRegisterable
	{
		public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
		{
			helper.Content.Cards.RegisterCard("CleoIsaacDemandCard", new()
			{
				CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
				Meta = new()
				{
					deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
					rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
					upgradesTo = [Upgrade.A, Upgrade.B],
					dontOffer = true,
				},
				Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Duo/CleoIsaacDemand.png")).Sprite,
				Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Duo", "CleoIsaacDemand", "name"]).Localize
			});
		}

		public override CardData GetData(State state)
			=> new()
			{
				cost = upgrade == Upgrade.A ? 1 : 2,
				temporary = true
			};

		public override List<CardAction> GetActions(State s, Combat c)
			=> [
				new ASpawn
				{
					thing = new AttackDrone
					{
						upgraded = upgrade == Upgrade.B
					},
					offset = -1
				},
				new ASpawn
				{
					thing = new AttackDrone
					{
						upgraded = upgrade == Upgrade.B
					},
					offset = 1
				}
			];
	}
}