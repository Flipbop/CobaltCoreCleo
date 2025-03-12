using System.Linq;
using Nanoray.PluginManager;

using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Flipbop.Cleo;

internal sealed class ApologizeNextLoopCard : Card, IRegisterable
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
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ApologizeNextLoop", "name"]).Localize
		});
	}

	public override CardData GetData(State state)
		=> new()
		{
			artTint = "FFFFFF",
			cost = 1,
			description = ModEntry.Instance.Localizations.Localize(["card", "ApologizeNextLoop", "description", upgrade.ToString()])
		};

	public override List<CardAction> GetActions(State s, Combat c)
		=> [
			new AStatus
			{
				targetPlayer = true,
				status = Status.shield,
				statusAmount = 1,
			},
			new PositionalAction
			{
				Leftmost = upgrade == Upgrade.A,
				Rightmost = true,
				Discount = true,
				Strengthen = upgrade == Upgrade.B,
			}
		];

	private sealed class PositionalAction : CardAction
	{
		public bool Leftmost;
		public bool Rightmost;
		public bool Discount = true;
		public bool Strengthen = false;

		public override void Begin(G g, State s, Combat c)
		{
			base.Begin(g, s, c);

			if (Leftmost && c.hand.FirstOrDefault() is { } leftmostCard)
			{
				if (Discount)
					leftmostCard.discount--;
				if (Strengthen)
					leftmostCard.AddImprovedA(1);
			}
			if (Rightmost && c.hand.LastOrDefault() is { } rightmostCard)
			{
				if (Discount)
					rightmostCard.discount--;
				if (Strengthen)
					rightmostCard.AddImprovedA(1);
			}
		}

		public override List<Tooltip> GetTooltips(State s)
		{
			var tooltips = new List<Tooltip>();
			if (Discount)
				tooltips.Add(new TTGlossary("cardtrait.discount", 1));
			if (Strengthen)
				tooltips.Add(ModEntry.Instance.Api.GetImprovedATooltip(1));
			return tooltips;
		}
	}
}
