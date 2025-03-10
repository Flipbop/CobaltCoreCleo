﻿using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class ExpensiveEquipmentArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Artifacts.RegisterArtifact("ExpensiveEquipment", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = ModEntry.Instance.CleoDeck.Deck,
				pools = ModEntry.GetArtifactPools(MethodBase.GetCurrentMethod()!.DeclaringType!)
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/ExpensiveEquipment.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ExpensiveEquipment", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ExpensiveEquipment", "description"]).Localize
		});
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> [new TTGlossary("cardtrait.temporary")];

	public override void OnTurnStart(State state, Combat combat)
	{
		base.OnTurnStart(state, combat);
		if (!combat.isPlayerTurn)
			return;

		combat.Queue([
			new ADelay(),
			new ACardOffering
			{
				amount = 2,
				makeAllCardsTemporary = true,
				overrideUpgradeChances = false,
				canSkip = false,
				inCombat = true,
				artifactPulse = Key()
			}
		]);
	}
}
