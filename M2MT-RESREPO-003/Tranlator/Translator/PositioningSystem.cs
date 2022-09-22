using System.Linq;
using Translator.Util;
using Models.TopoModels.EULYNX.rsmCommon;
using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.sig;
using System.Xml.Serialization;
using ProRail.IMSpoor.Model;
using Translator.Util;

namespace Translator 
{
	public class M2MTPositioningSystem : ITranslator
	{
		public void Translate(tSituation situation, EulynxDataPrepInterface eulynx, DataContainer dataContainer)
		{
			var posSys = new PositioningSystem()
			{
				id = IdManager.computeUuid5<PositioningSystem>("EPSG:28992"),
				name = "EPSG:28992",
			};
			
			dataContainer.ownsRsmEntities.usesTopography.usesPositioningSystem.Add(posSys);
		
		}
	}
}