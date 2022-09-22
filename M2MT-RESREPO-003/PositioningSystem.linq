<Query Kind="Program">
  <NuGetReference>EulynxDpLibrary</NuGetReference>
  <NuGetReference>ProRail.IMSpoor.Model</NuGetReference>
  <Namespace>Models.TopoModels.EULYNX.rsmCommon</Namespace>
  <Namespace>Models.TopoModels.EULYNX.generic</Namespace>
  <Namespace>Models.TopoModels.EULYNX.sig</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>ProRail.IMSpoor.Model</Namespace>
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
	// create reference system for Spotlocations
	var posSys = new PositioningSystem()
	{
		id = IdManager.computeUuid5<PositioningSystem>("EPSG:28992"),
		name = "EPSG:28992",
	};
	
	dataContainer.ownsRsmEntities.usesTopography.usesPositioningSystem.Add(posSys);

	// should be added seperatly?
	//posSys.Dump();
}