using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class ACleanSlate : DynamicWidthCardAction
{
	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		while (index >= 0)
		{
			if (c.hand[index].upgrade == Upgrade.None)
			{
				c.hand[index].ExhaustFX();
				Audio.Play(Event.CardHandling);
				c.hand.Remove(c.hand[index]);
				c.SendCardToExhaust(s, c.hand[index]);
			} 
			index--;
		}
	}
}
