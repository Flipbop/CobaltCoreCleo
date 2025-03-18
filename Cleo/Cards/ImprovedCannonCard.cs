using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Flipbop.Cleo;

internal sealed class ImprovedCannonCard : Card, IRegisterable
{
	public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
	{
		helper.Content.Cards.RegisterCard(MethodBase.GetCurrentMethod()!.DeclaringType!.Name, new()
		{
			CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
			Meta = new()
			{
				deck = ModEntry.Instance.CleoDeck.Deck,
				rarity = ModEntry.GetCardRarity(MethodBase.GetCurrentMethod()!.DeclaringType!),
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/colorless.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ImprovedCannon", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 2,
			
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> upgrade switch
		{
			Upgrade.A =>
			[
				new AUpgradeDiscardHint(),
				new AAttack { damage = GetDmg(s, c.discard.Count(card => card.upgrade != Upgrade.None)), xHint = 1},
			],
			Upgrade.B => [
				new AUpgradeExhaustHint(),
				new AAttack { damage = GetDmg(s, 2*(c.exhausted.Count(card => card.upgrade != Upgrade.None))), xHint = 2},
			],
			_ => [
				new AUpgradeHint(),
				new AAttack { damage = GetDmg(s, c.hand.Count(card => card.upgrade != Upgrade.None)), xHint = 1},
			]
			
		};
	
	public sealed class AUpgradeHint : AVariableHint
	{
		public override Icon? GetIcon(State s)
			=> new(ModEntry.Instance.UpgradesInHandIcon.Sprite, null, Colors.textMain);


		public override List<Tooltip> GetTooltips(State s)
			=> [new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::UpgradesInHand")
			{
				Icon = ModEntry.Instance.UpgradesInHandIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "UpgradesInHand", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "UpgradesInHand", "description"])
			}];
	}
	
	public sealed class AUpgradeDiscardHint : AVariableHint
	{
		public override Icon? GetIcon(State s)
			=> new(ModEntry.Instance.UpgradesInDiscardIcon.Sprite, null, Colors.textMain);


		public override List<Tooltip> GetTooltips(State s)
			=> [new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::UpgradesInDiscard")
			{
				Icon = ModEntry.Instance.UpgradesInDiscardIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "UpgradesInDiscard", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "UpgradesInDiscard", "description"])
			}];
	}
	public sealed class AUpgradeExhaustHint : AVariableHint
	{
		public override Icon? GetIcon(State s)
			=> new(ModEntry.Instance.UpgradesInExhaustIcon.Sprite, null, Colors.textMain);


		public override List<Tooltip> GetTooltips(State s)
			=> [new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::UpgradesInExhaust")
			{
				Icon = ModEntry.Instance.UpgradesInDiscardIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "UpgradesInExhaust", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "UpgradesInExhaust", "description"])
			}];
	}
}
