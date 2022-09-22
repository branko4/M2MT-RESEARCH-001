<Query Kind="Program">
  <NuGetReference>EulynxDpLibrary</NuGetReference>
  <NuGetReference>ProRail.IMSpoor.Model</NuGetReference>
  <Namespace>Models.TopoModels.EULYNX.generic</Namespace>
  <Namespace>Models.TopoModels.EULYNX.sig</Namespace>
  <Namespace>Models.TopoModels.EULYNX.rsmCommon</Namespace>
  <Namespace>ProRail.IMSpoor.Model</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>Translator.Util</Namespace>
</Query>

#load ".\Tranlator\Translator\Util\TranslationUtil.cs"

void Main()
{
	// setup
	var eulynx = new EulynxDataPrepInterface();
	var dataContainer = TranslationUtil.getEmptyDataContainer();
	var situation = TranslationUtil.GetIMSpoor();

	// query
	Translate(situation, eulynx, dataContainer);

	// finish
	eulynx.hasDataContainer = new List<DataContainer> { dataContainer };
	eulynx.Dump();
	TranslationUtil.WriteToFile(eulynx);
}

void Translate(tSituation situation, EulynxDataPrepInterface eulynx, DataContainer dataContainer)
{
	
	var microLinkQuery =
	from microLink in situation.RailInfrastructure.RailTopology.MicroLinks
	where microLink is ProRail.IMSpoor.Model.MicroLink
	//where bufferstopNodes.Contains(microLink.ToMicroNode.nodeRef) || bufferstopNodes.Contains(microLink.FromMicroNode.nodeRef)
	select new PositioningNetElement()
	{
		id = IdManager.computeUuid5<PositioningNetElement>(microLink.ToMicroNode.nodeRef + microLink.FromMicroNode.nodeRef),
	};

	microLinkQuery.Dump();
	
	dataContainer.ownsRsmEntities.usesTrackTopology.usesNetElement.AddRange(microLinkQuery.ToList());
}
