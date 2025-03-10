using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Linq;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class UpgradedTerminalArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Artifacts.RegisterArtifact("UpgradedTerminal", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = ModEntry.Instance.CleoDeck.Deck,
				pools = ModEntry.GetArtifactPools(MethodBase.GetCurrentMethod()!.DeclaringType!)
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/UpgradedTerminal.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "UpgradedTerminal", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "UpgradedTerminal", "description"]).Localize
		});

		DB.story.all[$"ShopkeeperInfinite_{ModEntry.Instance.CleoDeck.Deck.Key()}_UpgradedTerminal"] = new()
		{
			type = NodeType.@event,
			bg = "BGShop",
			lines = [
				new CustomSay()
				{
					who = ModEntry.Instance.CleoDeck.Deck.Key(),
					AlternativeTexts = ModEntry.Instance.Localizations.Localize(["artifact", "UpgradedTerminal", "dialogue"]).Split("\n").ToList(),
					loopTag = "neutral"
				}
			]
		};

		ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(MapShop), nameof(MapShop.MakeRoute)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(MapShop_MakeRoute_Postfix))
		);
	}

	public override void OnReceiveArtifact(State state)
	{
		base.OnReceiveArtifact(state);
		state.ship.baseEnergy++;
	}

	private static void MapShop_MakeRoute_Postfix(State s, ref Route __result)
	{
		if (s.EnumerateAllArtifacts().FirstOrDefault(a => a is UpgradedTerminalArtifact) is not { } artifact)
			return;
		__result = Dialogue.MakeDialogueRouteOrSkip(s, DB.story.QuickLookup(s, $"ShopkeeperInfinite_{ModEntry.Instance.CleoDeck.Deck.Key()}_UpgradedTerminal"), OnDone.map);
		artifact.Pulse();
	}
}
