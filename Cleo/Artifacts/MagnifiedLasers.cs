using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class MagnifiedLasersArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Artifacts.RegisterArtifact("MagnifiedLasers", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = ModEntry.Instance.CleoDeck.Deck,
				pools = ModEntry.GetArtifactPools(MethodBase.GetCurrentMethod()!.DeclaringType!)
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/MagnifiedLasers.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MagnifiedLasers", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MagnifiedLasers", "description"]).Localize
		});
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> new List<Tooltip> { new TTCard { card = new SmallRepairsCard() } }
			.Concat(new SmallRepairsCard().GetAllTooltips(MG.inst.g, DB.fakeState))
			.ToList();

	public override void OnTurnStart(State state, Combat combat)
	{
		base.OnTurnStart(state, combat);
		if (!combat.isPlayerTurn || combat.turn != 1)
			return;

		combat.Queue(new AAddCard
		{
			destination = CardDestination.Hand,
			card = new SmallRepairsCard(),
			artifactPulse = Key()
		});
	}
}
