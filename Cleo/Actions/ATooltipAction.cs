using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flipbop.Cleo;

internal sealed class ATooltipAction : ADummyAction
{
	[JsonIgnore]
	public List<Tooltip>? Tooltips;

	public override void Begin(G g, State s, Combat c)
	{
		base.Begin(g, s, c);
		timer = 0;
	}

	public override List<Tooltip> GetTooltips(State s)
		=> Tooltips ?? [];
}
