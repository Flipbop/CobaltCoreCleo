using System.Collections.Generic;
using Nickel;
using System.Linq;
using FSPRO;
using Shockah.Kokoro;

namespace Flipbop.Cleo;
internal sealed class ImpairedCostManager
{
    internal static readonly ICardTraitEntry Trait = ModEntry.Instance.ImpairedTrait;
	
    public ImpairedCostManager()
    {
        ModEntry.Instance.KokoroApi.ActionCosts.RegisterHook(new ImpairedCostHook());
        ModEntry.Instance.KokoroApi.ActionCosts.RegisterResourceCostIcon(new ImpairedCost(), ModEntry.Instance.ImpairedIcon.Sprite, ModEntry.Instance.ImpairedIcon.Sprite);
    }
}

internal sealed class ImpairedCost : IKokoroApi.IV2.IActionCostsApi.IResource
{
    public string ResourceKey => "Cleo::Impaired";
    public int GetCurrentResourceAmount(State state, Combat combat)
    {
        int index = combat.hand.Count -1;
        int upgradeCounter = 0;
        int cardCounter = 0;
        int? currentCard = ModEntry.Instance.helper.ModData.ObtainModData<int?>(combat, "Card");
        while (index >= 0)
        {
            if (combat.hand[index].uuid == currentCard)
            {
                continue;
            }
            cardCounter++;
            if (combat.hand[index].upgrade != Upgrade.None)
            {
                upgradeCounter++;
            }
            index--;
        }
        return upgradeCounter;
    }

    public void Pay(State s, Combat c, int amount)
    {
        int index = c.hand.Count -1;
	    while (index >= 0 && amount > 0)
	    {
		    if (c.hand[index].upgrade != Upgrade.None)
		    {
			    if (!c.hand[index].GetImprovedA() && !c.hand[index].GetImprovedB())
			    {
				    ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImpairedTrait, true, false);
				    ImpairedExt.AddImpaired(c.hand[index], s);
				    amount--;
				    Audio.Play(Event.CardHandling);
			    }
			    else
			    {
				    ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedATrait, false, false);
				    ImprovedAExt.RemoveImprovedA(c.hand[index], s);
				    ModEntry.Instance.helper.Content.Cards.SetCardTraitOverride(s, c.hand[index], ModEntry.Instance.ImprovedBTrait, false, false);
				    ImprovedBExt.RemoveImprovedB(c.hand[index], s);
				    amount--;
				    Audio.Play(Event.CardHandling);
			    }
		    }
		    index--;
	    }
    }

    public IReadOnlyList<Tooltip> GetTooltips(State state, Combat combat, int amount)
    {
        return [];
    }
}

internal sealed class ImpairedCostHook : IKokoroApi.IV2.IActionCostsApi.IHook
{
    bool ModifyActionCost(IKokoroApi.IV2.IActionCostsApi.IHook.IModifyActionCostArgs args)
    {
        ModEntry.Instance.helper.ModData.SetModData(args.Combat, "Card", args.Card?.uuid);
        return false;
    }
}
