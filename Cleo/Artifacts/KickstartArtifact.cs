using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class KickstartArtifact : Artifact, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Artifacts.RegisterArtifact("Kickstart", new()
		{
			ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				owner = ModEntry.Instance.CleoDeck.Deck,
				pools = ModEntry.GetArtifactPools(MethodBase.GetCurrentMethod()!.DeclaringType!)
			},
			Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifacts/Kickstart.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Kickstart", "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Kickstart", "description"]).Localize
		});
	}

	public override List<Tooltip>? GetExtraTooltips()
		=> [new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Improve A")
			{
				Icon = ModEntry.Instance.ImprovedIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "ImproveA", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "ImproveA", "description"])
			}];

	public override void OnCombatStart(State state, Combat combat)
	{
		base.OnCombatStart(state, combat);
		int Amount = 2;
		int index = state.deck.Count -1;
		while (index >= 0 && Amount > 0)
		{
			if (state.deck[index].upgrade == Upgrade.None && state.deck[index].IsUpgradable())
			{
				ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(state, state.deck[index], ModEntry.Instance.ImprovedATrait, true, false);
				ImprovedAExt.AddImprovedA(state.deck[index], state);
				Amount--;
			}
			index--;
		}
	}
}
