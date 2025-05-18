using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class CleoJohnsonArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		Deck johnsonDeck = ModEntry.Instance.IJohnsonApi.JohnsonDeck.Deck;
		if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;

		helper.Content.Artifacts.RegisterArtifact("CleoJohnson", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = api.DuoArtifactVanillaDeck,
				pools = [ArtifactPool.Common]
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/Duo/CleoCat.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Duo", "CleoJohnson", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Duo", "CleoJohnson", "description"]).Localize
		});

		api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.CleoDeck.Deck, johnsonDeck]);

		Hold0Card.Register(package, helper);
		Hold1Card.Register(package, helper);
		Hold2Card.Register(package, helper);
		Hold3Card.Register(package, helper);
		ReturnOnInvestmentCard.Register(package, helper);
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> [new TTCard { card = new Hold0Card() }];

	public override void OnReceiveArtifact(State state)
	{
		base.OnReceiveArtifact(state);
		state.deck.Add(new Hold0Card());
	}
}

internal sealed class Hold0Card : Card
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = true,
			},
			Art = helper.Content.Sprites
				.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/TurtleShot.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["Duo", "Hold0", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "996699",
			cost = 0,
			unplayable = upgrade == Upgrade.None,
			description = upgrade == Upgrade.None ? ModEntry.Instance.Localizations.Localize(["Duo", "Hold0", "description0", upgrade.ToString()]) : ModEntry.Instance.Localizations.Localize(["Duo", "Hold0", "description1", upgrade.ToString()]),
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=>
		[
			new AAddCard() {amount = 0, card = new Hold1Card()}
		];
}internal sealed class Hold1Card : Card
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = true,
			},
			Art = helper.Content.Sprites
				.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/TurtleShot.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["Duo", "Hold1", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "996699",
			cost = 0,
			unplayable = upgrade == Upgrade.None,
			description = upgrade == Upgrade.None ? ModEntry.Instance.Localizations.Localize(["Duo", "Hold1", "description0", upgrade.ToString()]) : ModEntry.Instance.Localizations.Localize(["Duo", "Hold1", "description1", upgrade.ToString()]),
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=>
		[
			new AAddCard() {amount = 1, card = new Hold2Card()}
		];
}internal sealed class Hold2Card : Card
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = true,
			},
			Art = helper.Content.Sprites
				.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/TurtleShot.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["Duo", "Hold2", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "996699",
			cost = 0,
			unplayable = upgrade == Upgrade.None,
			description = upgrade == Upgrade.None ? ModEntry.Instance.Localizations.Localize(["Duo", "Hold2", "description0", upgrade.ToString()]) : ModEntry.Instance.Localizations.Localize(["Duo", "Hold2", "description1", upgrade.ToString()]),
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=>
		[
			new AAddCard() {amount = 1, card = new Hold3Card()}
		];
}internal sealed class Hold3Card : Card
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = true,
			},
			Art = helper.Content.Sprites
				.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/TurtleShot.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["Duo", "Hold3", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "996699",
			cost = 0,
			unplayable = upgrade == Upgrade.None,
			description = upgrade == Upgrade.None ? ModEntry.Instance.Localizations.Localize(["Duo", "Hold3", "description0", upgrade.ToString()]) : ModEntry.Instance.Localizations.Localize(["Duo", "Hold3", "description1", upgrade.ToString()]),
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=>
		[
			new AAddCard() {amount = 1, card = new ReturnOnInvestmentCard()}
		];
}internal sealed class ReturnOnInvestmentCard : Card
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = true,
			},
			Art = helper.Content.Sprites
				.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/TurtleShot.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["Duo", "ReturnOnInvestment", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "996699",
			cost = 1,
			exhaust = true,
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.A =>
			[
				new AAttack { damage = GetDmg(s, 9) },
			],
			Upgrade.B => [
				new AStatus { targetPlayer = true, status = Status.shield, statusAmount = 2 },
				new AAttack { damage = GetDmg(s, 7) },
			],
			_ => [
				new AAttack { damage = GetDmg(s, 7) },
			]
		};
}