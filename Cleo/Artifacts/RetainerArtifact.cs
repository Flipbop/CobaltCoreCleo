using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class RetainerArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Artifacts.RegisterArtifact("Retainer", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = ModEntry.Instance.CleoDeck.Deck,
				pools = ModEntry.GetArtifactPools(MethodBase.GetCurrentMethod()!.DeclaringType!)
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/Retainer.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Retainer", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Retainer", "description"]).Localize
		});

		ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(State), nameof(State.SendCardToDeck)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(State_SendCardToDeck_Postfix))
		);
	}
	
	public override List<Tooltip>? GetExtraTooltips()
		=> [
			new TTGlossary("cardtrait.discount", 1),
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.UpgradeTooltip,
		];

	public override void OnCombatStart(State state, Combat combat)
	{
		base.OnCombatStart(state, combat);
	}

	private static void State_SendCardToDeck_Postfix(State __instance, Card card)
	{
		if (__instance.IsOutsideRun())
			return;
		if (__instance.route is Combat combat && !combat.EitherShipIsDead(__instance))
			return;
		if (__instance.EnumerateAllArtifacts().FirstOrDefault(a => a is RetainerArtifact) is not { } artifact)
			return;
		if (ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(card, "Retainer"))
			return;

		artifact.Pulse();
		ModEntry.Instance.Helper.ModData.SetModData(card, "Retainer", true);
		card.discount--;
		if (card.IsUpgradable())
			__instance.GetCurrentQueue().QueueImmediate(ModEntry.Instance.KokoroApi.TemporaryUpgrades.MakeChooseTemporaryUpgradeAction(card.uuid).AsCardAction);
	}
}
