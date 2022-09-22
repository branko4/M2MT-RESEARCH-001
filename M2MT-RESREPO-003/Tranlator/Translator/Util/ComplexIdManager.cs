using Models.TopoModels.EULYNX.rsmCommon;
using ProRail.IMSpoor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Util
{
    internal class ComplexIdManager
    {
		private static Dictionary<string, List<string>> PuicWithId = new Dictionary<string, List<string>>();

		public static void Init(tSituation situation)
        {
			var microLinkQuery =
			from microLink in situation.RailInfrastructure.RailTopology.MicroLinks
			where microLink is MicroLink
			select new
			{
				id = IdManager.computeUuid5<PositioningNetElement>(microLink.ToMicroNode.nodeRef + microLink.FromMicroNode.nodeRef),
				puic1 = microLink.ToMicroNode.nodeRef,
				puic2 = microLink.FromMicroNode.nodeRef,
			};

			foreach (var x in microLinkQuery.ToList())
			{
				if (!PuicWithId.ContainsKey(x.puic1)) PuicWithId[x.puic1] = new List<string>();
				if (!PuicWithId.ContainsKey(x.puic2)) PuicWithId[x.puic2] = new List<string>();
				PuicWithId[x.puic1].Add(x.id);
				PuicWithId[x.puic2].Add(x.id);
			}
		}

		public static string getNetElementIDForBufferstop(string puic)
		{
			if (PuicWithId[puic].Count != 1 ) Console.Error.WriteLine("Error more then 1");
			return PuicWithId[puic].FirstOrDefault();
		}
	}
}
