using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class NanomachinesCard : Card, IRegisterable
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
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Kickstart.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Nanomachines", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 0,
			buoyant = true,
			exhaust = true,
			description = ModEntry.Instance.Localizations.Localize(["card", "Nanomachines", "description", upgrade.ToString()])
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> [
			new ACardSelect
			{
				browseSource = CardBrowse.Source.Deck,
				browseAction = new TemporarilyUpgradeBrowseAction
				{
					Discount = this.upgrade == Upgrade.A ? -1 : 0,
					Strengthen = this.upgrade == Upgrade.B ? 1 : 0
				},
				filterUpgrade = Upgrade.None,
			},
			new ATooltipAction
			{
				Tooltips = new TemporarilyUpgradeBrowseAction
				{
					Discount = this.upgrade == Upgrade.A ? -1 : 0,
					Strengthen = this.upgrade == Upgrade.B ? 1 : 0
				}.GetTooltips(s)
			}
		];
}
