using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class CleoRiggsArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;

		helper.Content.Artifacts.RegisterArtifact("CleoRiggs", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = api.DuoArtifactVanillaDeck,
				pools = [ArtifactPool.Common]
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/Duo/CleoRiggs.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Duo", "CleoRiggs", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Duo", "CleoRiggs", "description"]).Localize
		});

		api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.CleoDeck.Deck, Deck.riggs]);
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> StatusMeta.GetTooltips(Status.evade, 1);

	public int UpgradeCount = 0;
	private static void OnDrawCard(State state, Combat combat, int count)
	{
		if (combat.hand[^1].upgrade != Upgrade.None)
		{
			
		}
	}
}