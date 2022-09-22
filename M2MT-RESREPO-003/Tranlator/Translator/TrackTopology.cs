using System.Linq;
using Translator.Util;
using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.sig;
using Models.TopoModels.EULYNX.rsmCommon;
using ProRail.IMSpoor.Model;
using System.Xml.Serialization;
using Translator.Util;

namespace Translator 
{
	public class M2MTTrackTopology : ITranslator
	{
		public void Translate(tSituation situation, EulynxDataPrepInterface eulynx, DataContainer dataContainer)
		{
			
			var microLinkQuery =
			from microLink in situation.RailInfrastructure.RailTopology.MicroLinks
			where microLink is ProRail.IMSpoor.Model.MicroLink
			select new PositioningNetElement()
			{
				id = IdManager.computeUuid5<PositioningNetElement>(microLink.ToMicroNode.nodeRef + microLink.FromMicroNode.nodeRef),
			};
		
			
			dataContainer.ownsRsmEntities.usesTrackTopology.usesNetElement.AddRange(microLinkQuery.ToList());
		}
		
	}
}