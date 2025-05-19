
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic
{
    public interface IPhilipAPI
    {
        IDeckEntry PhilipDeck { get; }
        IStatusEntry CustomPartsStatus { get; }

        enum CardModifierTarget
        {
            Directional,
            Directional_WholeHand,
            Neighboring,
            All
        }

        enum CardModifierTargetDirection
        {
            RIGHT,
            LEFT
        }

        struct AModifierWrapperMeta
        {
            public bool isFlimsy;
            public CardModifierTargetDirection direction;
        }
        
        CardModifier MakeMAddAction(CardAction action, Spr? stickerSprite);
        CardModifier MakeMBuffAttack(int amount);
        CardModifier MakeMAttacksPierce();
        CardModifier MakeMStun();
        CardModifier MakeMDeleteActions();
        CardModifier MakeMPlayTwice();

        CardModifier MakeMExhaust();
        CardModifier MakeMRetain();
        CardModifier MakeMPlayable();
        CardModifier MakeMRecycle();
        CardModifier MakeMSetEnergyCostToZero();
        CardModifier MakeMReduceEnergyCost();
    }

    public abstract class CardModifier
    {
        public Deck sourceDeck = Deck.colorless;
        // public List<int> flimsyUuids = [];
        public int sourceUuid;
        public bool fromFlimsy;
        public abstract double Priority { get; }
        public abstract bool IgnoresFlimsy { get; }
        virtual public bool RequestsStickyNote() { return false; }
        virtual public bool MandatesStickyNote() { return false; }
        virtual public Spr? GetSticker(State s) { return null; }
        // virtual public Icon? GetIcon(State s) { return null; }
        public abstract CardAction GetActionForRendering(State s);
        virtual public List<Tooltip> GetTooltips(State s) { return []; }
    }

    public interface ICardActionModifier
    {
        List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering) 
            => TransformActions(actions, s, c, card, isRendering, out _);
        List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering, out bool success);
    }

    public interface ICardDataModifier
    {
        CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
            => TransformData(data, s, c, card, isRendering, out _);
        CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering, out bool success);
    }
}
