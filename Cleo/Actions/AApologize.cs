using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AApologize : DynamicWidthCardAction
{
	public required int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		int dmg = 0;
		while (index >= 0)
		{
			if (c.hand[index].upgrade != Upgrade.None)
			{
				dmg++;
				c.Queue(new AAttack { damage = Card.GetActualDamage(s, dmg) });
			}
			index--;
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairHandIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);
	
}
