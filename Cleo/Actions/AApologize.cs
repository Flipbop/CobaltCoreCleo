using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class AApologize : DynamicWidthCardAction
{
	public required int dmgRamp;
	public required bool peirce;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.hand.Count -1;
		int dmg = 1;
		int upgradeCounter = 0;
		while (index >= 0)
		{
			if (c.hand[index].upgrade != Upgrade.None)
			{
				upgradeCounter++;
			}
			index--;
		}

		for (int i = 0; i < upgradeCounter; i++)
		{
			c.Queue(new AAttack { damage = Card.GetActualDamage(s, dmg), piercing = peirce});
			dmg += dmgRamp;
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairHandIcon.Sprite, null, Colors.textMain);
	
}
