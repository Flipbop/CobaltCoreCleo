using FSPRO;
using Nickel;
using System.Collections.Generic;


namespace Flipbop.Cleo;

public sealed class ASeekerBarrageDiscard : DynamicWidthCardAction
{
	public required int Amount;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		int index = c.discard.Count -1;
		while (index >= 0)
		{
			if (c.discard[index].upgrade != Upgrade.None)
			{
				c.Queue(new AMove{dir= 1});
				c.Queue(new ASpawn{fromPlayer = true, thing = new Missile{missileType = MissileType.seeker}});
			}
			index--;
		}
	}

	public override Icon? GetIcon(State s)
		=> new(ModEntry.Instance.ImpairHandIcon.Sprite, Amount == -1 ? null : Amount, Colors.textMain);
	
}
