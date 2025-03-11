using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Nickel.Common;
using Shockah.Kokoro;
using System;
using System.Collections.Generic;
using System.Linq;
using Flipbop.Cleo;

namespace Flipbop.Cleo;

public sealed class ModEntry : SimpleMod
{
	internal static ModEntry Instance { get; private set; } = null!;
	internal readonly ICleoApi Api = new ApiImplementation();

	internal IHarmony Harmony { get; }
	internal IKokoroApi.IV2 KokoroApi { get; }
	internal IDuoArtifactsApi? DuoArtifactsApi { get; }
	internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
	internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

	internal IDeckEntry CleoDeck { get; }
	internal IPlayableCharacterEntryV2 CleoCharacter { get; }
	internal IStatusEntry CrunchTimeStatus { get; }

	internal ISpriteEntry ImproveAIcon { get; }
	internal ISpriteEntry ImproveBIcon { get; }
	internal ISpriteEntry ImpairedIcon { get; }
	internal ISpriteEntry ImprovedIcon { get; }
	internal ISpriteEntry DiscountHandIcon { get; }

	internal static IReadOnlyList<Type> CommonCardTypes { get; } = [
		typeof(QuickBoostCard),
		typeof(TurtleShotCard),
		typeof(ChoicesCard),
//		typeof(MemoryRecoveryCard),
		typeof(ShuffleUpgradeCard),
//		typeof(ResourceSwapCard),
		typeof(ReroutePowerCard),
//		typeof(NanomachinesCard),
		typeof(SlipShotCard),
	];

	internal static IReadOnlyList<Type> UncommonCardTypes { get; } = [
		typeof(PowerSurgeCard),
		typeof(ImprovedCannonCard),
		typeof(DoItYourselfCard),
		typeof(RepairedGlassesCard),
//		typeof(ScalpedPartsCard),
		typeof(SwapNotesCard), 
//		typeof(PowerSwitchCard),
	];

	internal static IReadOnlyList<Type> RareCardTypes { get; } = [
//		typeof(SeekerBarrageCard),
//		typeof(HarnessEnergyCard),
//		typeof(CleanSlateCard),
//		typeof(ApologizeNextLoopCard),
		typeof(HardResetCard),
	];

	internal static IReadOnlyList<Type> SpecialCardTypes { get; } = [
		typeof(SmallRepairsCard),
	];

	internal static IEnumerable<Type> AllCardTypes { get; }
		= [..CommonCardTypes, ..UncommonCardTypes, ..RareCardTypes, typeof(CleoExeCard), ..SpecialCardTypes];

	internal static IReadOnlyList<Type> CommonArtifacts { get; } = [
/*		typeof(EnhancedToolsArtifact),
		typeof(ReusableMaterialsArtifact),
		typeof(KickstartArtifact),
		typeof(MagnifiedLasersArtifact),
		typeof(UpgradedTerminalArtifact), */
	];

	internal static IReadOnlyList<Type> BossArtifacts { get; } = [
/*		typeof(RetainerArtifact),
		typeof(ExpensiveEquipmentArtifact),
		typeof(PowerEchoArtifact), */
	];

	internal static IReadOnlyList<Type> DuoArtifacts { get; } = [
/*		typeof(CleoBooksArtifact),
		typeof(CleoCatArtifact),
		typeof(CleoDizzyArtifact),
		typeof(CleoDrakeArtifact),
		typeof(CleoIsaacArtifact),
		typeof(CleoMaxArtifact),
		typeof(CleoPeriArtifact),
		typeof(CleoRiggsArtifact), */
	];

	internal static IEnumerable<Type> AllArtifactTypes
		=> [..CommonArtifacts, ..BossArtifacts];

	internal static readonly IEnumerable<Type> RegisterableTypes
		= [..AllCardTypes, ..AllArtifactTypes];

	internal static readonly IEnumerable<Type> LateRegisterableTypes
		= DuoArtifacts;

	public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
	{
		Instance = this;
		Harmony = helper.Utilities.Harmony;
		KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
		DuoArtifactsApi = helper.ModRegistry.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts");

		helper.Events.OnModLoadPhaseFinished += (_, phase) =>
		{
			if (phase != ModLoadPhase.AfterDbInit)
				return;

			foreach (var registerableType in LateRegisterableTypes)
				AccessTools.DeclaredMethod(registerableType, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
		};

		this.AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/main-{locale}.json").OpenRead()
		);
		this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
		);

		_ = new CrunchTimeManager();
		_ = new ImprovedAManager();
		_ = new ImprovedBManager();
		CardSelectFilters.Register(package, helper);

		DynamicWidthCardAction.ApplyPatches(Harmony, logger);

		CrunchTimeStatus = helper.Content.Statuses.RegisterStatus("Clean Up", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Icons/CleanUp.png")).Sprite,
				color = new("F7883E"),
				isGood = true
			},
			Name = this.AnyLocalizations.Bind(["status", "CleanUp", "name"]).Localize,
			Description = this.AnyLocalizations.Bind(["status", "CleanUp", "description"]).Localize
		});

		CleoDeck = helper.Content.Decks.RegisterDeck("Cleo", new()
		{
			Definition = new() { color = new("8A3388"), titleColor = Colors.white },
			DefaultCardArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Default.png")).Sprite,
			BorderSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CardFrame.png")).Sprite,
			Name = this.AnyLocalizations.Bind(["character", "name"]).Localize
		});

		foreach (var registerableType in RegisterableTypes)
			AccessTools.DeclaredMethod(registerableType, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);

		CleoCharacter = helper.Content.Characters.V2.RegisterPlayableCharacter("Cleo", new()
		{
			Deck = CleoDeck.Deck,
			Description = this.AnyLocalizations.Bind(["character", "description"]).Localize,
			BorderSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CharacterFrame.png")).Sprite,
			NeutralAnimation = new()
			{
				CharacterType = CleoDeck.UniqueName,
				LoopTag = "neutral",
				Frames = Enumerable.Range(0, 5)
					.Select(i => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/Character/Neutral/{i}.png")).Sprite)
					.ToList()
			},
			MiniAnimation = new()
			{
				CharacterType = CleoDeck.UniqueName,
				LoopTag = "mini",
				Frames = [
					helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Character/mini.png")).Sprite
				]
			},
			Starters = new()
			{
				cards = [
					new QuickBoostCard(),
					new TurtleShotCard()
				]
			},
			ExeCardType = typeof(CleoExeCard)
		});

		helper.Content.Characters.V2.RegisterCharacterAnimation(new()
		{
			CharacterType = CleoDeck.UniqueName,
			LoopTag = "gameover",
			Frames = Enumerable.Range(0, 1)
				.Select(i => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/Character/GameOver/{i}.png")).Sprite)
				.ToList()
		});
		helper.Content.Characters.V2.RegisterCharacterAnimation(new()
		{
			CharacterType = CleoDeck.UniqueName,
			LoopTag = "squint",
			Frames = Enumerable.Range(0, 3)
				.Select(i => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/Character/Squint/{i}.png")).Sprite)
				.ToList()
		});
		helper.Content.Characters.V2.RegisterCharacterAnimation(new()
		{
			CharacterType = CleoDeck.UniqueName,
			LoopTag = "explaining",
			Frames = Enumerable.Range(0, 5)
				.Select(i => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/Character/Explain/{i}.png")).Sprite)
				.ToList()
		});
		helper.Content.Characters.V2.RegisterCharacterAnimation(new()
		{
			CharacterType = CleoDeck.UniqueName,
			LoopTag = "nervous",
			Frames = Enumerable.Range(0, 5)
				.Select(i => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/Character/Nervous/{i}.png")).Sprite)
				.ToList()
		});
		helper.Content.Characters.V2.RegisterCharacterAnimation(new()
		{
			CharacterType = CleoDeck.UniqueName,
			LoopTag = "happy",
			Frames = Enumerable.Range(0, 4)
				.Select(i => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/Character/Happy/{i}.png")).Sprite)
				.ToList()
		});

		ImproveAIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Icons/ImproveA.png"));
		ImproveBIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Icons/ImproveB.png"));
		ImpairedIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Icons/Impaired.png"));
		ImprovedIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Icons/Improved.png"));
		DiscountHandIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Icons/DiscountHand.png"));

		helper.ModRegistry.AwaitApi<IMoreDifficultiesApi>(
			"TheJazMaster.MoreDifficulties",
			new SemanticVersion(1, 3, 0),
			api => api.RegisterAltStarters(
				deck: CleoDeck.Deck,
				starterDeck: new StarterDeck
				{
					cards = [
						new ShuffleUpgradeCard(),
						new SlipShotCard()
					]
				}
			)
		);

		_ = new DialogueExtensions();
		_ = new CombatDialogue();
		_ = new EventDialogue();
		_ = new CardDialogue();
	}

	public override object? GetApi(IModManifest requestingMod)
		=> new ApiImplementation();

	internal static Rarity GetCardRarity(Type type)
	{
		if (RareCardTypes.Contains(type))
			return Rarity.rare;
		if (UncommonCardTypes.Contains(type))
			return Rarity.uncommon;
		return Rarity.common;
	}

	internal static ArtifactPool[] GetArtifactPools(Type type)
	{
		if (BossArtifacts.Contains(type))
			return [ArtifactPool.Boss];
		if (CommonArtifacts.Contains(type))
			return [ArtifactPool.Common];
		return [];
	}
}
