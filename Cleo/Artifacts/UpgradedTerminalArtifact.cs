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
	}

	
	private int upgradeCount = 0;
	private int drawCount = 0;
	public void OnDrawCard(State state, Combat combat)
	{
		base.OnDrawCard(state, combat, 1);
		
		if (combat.hand[combat.hand.Count - 1].upgrade != Upgrade.None)
		{
			upgradeCount++;
			if (upgradeCount >= 2 && drawCount <= 3)
			{
				upgradeCount = 0;
				combat.Queue([
					new ADrawCard()
				]);
				drawCount++;
			}
		}
	}
	public override void OnTurnEnd(State state, Combat combat)
	{
		base.OnTurnEnd(state, combat);
		if (!combat.isPlayerTurn)
			return;
		drawCount = 0;
	}
	public override void OnCombatEnd(State state)
	{
		base.OnCombatEnd(state);
		drawCount = 0;
		upgradeCount = 0;
	}

	public override int? GetDisplayNumber(State s)
	{
		return upgradeCount;
	}
}
