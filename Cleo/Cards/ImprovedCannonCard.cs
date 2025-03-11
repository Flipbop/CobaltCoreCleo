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
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cards/Mint.png")).Sprite,
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
				new AVariableHint {xHint = c.discard.Count(card => card.upgrade != Upgrade.None) + s.deck.Count(card => card.upgrade != Upgrade.None)},
				new AAttack { damage = GetDmg(s, c.discard.Count(card => card.upgrade != Upgrade.None) + s.deck.Count(card => card.upgrade != Upgrade.None)), xHint = 1},
			],
			Upgrade.B => [
				new AVariableHint {xHint = 2*(c.exhausted.Count(card => card.upgrade != Upgrade.None))},
				new AAttack { damage = GetDmg(s, 2*(c.exhausted.Count(card => card.upgrade != Upgrade.None))), xHint = 2},
			],
			_ => [
				new AVariableHint {xHint = c.hand.Count(card => card.upgrade != Upgrade.None)},
				new AAttack { damage = GetDmg(s, c.hand.Count(card => card.upgrade != Upgrade.None)), xHint = 1},
			]
			
		};
		

	
	
	
	
	
	
	
	
	public sealed class UpgradeNonPermanentlyUpgradedCardBrowseAction : CardAction
	{
		public override List<Tooltip> GetTooltips(State s)
			=> [new TTGlossary("action.upgradeCard")];

		public override Route? BeginWithRoute(G g, State s, Combat c)
		{
			timer = 0;
			var baseResult = base.BeginWithRoute(g, s, c);
			if (selectedCard is null)
				return baseResult;

			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(selectedCard, null);
			return new CardUpgrade
			{
				cardCopy = Mutil.DeepCopy(selectedCard)
			};
		}
	}

	public sealed class MakeUpgradePermanentBrowseAction : CardAction
	{
		public override List<Tooltip> GetTooltips(State s)
			=> [
				new TTGlossary("action.upgradeCard"),
				ModEntry.Instance.KokoroApi.TemporaryUpgrades.UpgradeTooltip,
			];

		public override void Begin(G g, State s, Combat c)
		{
			base.Begin(g, s, c);
			timer = 0;
			if (selectedCard is null)
				return;
			if (ModEntry.Instance.KokoroApi.TemporaryUpgrades.GetTemporaryUpgrade(selectedCard) is not { } temporaryUpgrade)
				return;
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetTemporaryUpgrade(selectedCard, null);
			ModEntry.Instance.KokoroApi.TemporaryUpgrades.SetPermanentUpgrade(selectedCard, temporaryUpgrade);
		}
	}
}
