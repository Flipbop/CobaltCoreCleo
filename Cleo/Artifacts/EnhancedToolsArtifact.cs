using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class EnhancedToolsArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Artifacts.RegisterArtifact("EnhancedTools", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = ModEntry.Instance.CleoDeck.Deck,
				pools = ModEntry.GetArtifactPools(MethodBase.GetCurrentMethod()!.DeclaringType!)
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/EnhancedTools.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "EnhancedTools", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "EnhancedTools", "description"]).Localize
		});
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> [
			new TTCard { card = new SmallRepairsCard() },
			new TTCard { card = new SmallRepairsCard() },
			new TTCard { card = new SmallRepairsCard() },
			new TTCard { card = new SmallRepairsCard() },
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
				Destination = CardDestination.Hand,
				Cards = [
					new SmallRepairsCard()
				],
				artifactPulse = Key()
			}
		]);
	}
}
