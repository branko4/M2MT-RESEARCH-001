<Query Kind="Program">
  <NuGetReference>EulynxDpLibrary</NuGetReference>
  <NuGetReference>ProRail.IMSpoor.Model</NuGetReference>
  <Namespace>ProRail.IMSpoor.Model</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>Models.TopoModels.EULYNX.rsmCommon</Namespace>
  <Namespace>Models.TopoModels.EULYNX.rsmTrack</Namespace>
  <Namespace>Models.TopoModels.EULYNX.sig</Namespace>
  <Namespace>Models.TopoModels.EULYNX.generic</Namespace>
  <Namespace>Translator.Util</Namespace>
</Query>

#load ".\Tranlator\Translator\Util\TranslationUtil.cs"

string getNetElementIDForBufferstop(string puic)
{
	// setup
	var eulynx = new EulynxDataPrepInterface();
	var dataContainer = TranslationUtil.getEmptyDataContainer();
	var situation = TranslationUtil.GetIMSpoor();

	var microLinkQuery =
	from microLink in situation.RailInfrastructure.RailTopology.MicroLinks
	where microLink is ProRail.IMSpoor.Model.MicroLink
	where microLink.ToMicroNode.nodeRef == puic || microLink.FromMicroNode.nodeRef == puic
	select IdManager.computeUuid5<PositioningNetElement>(microLink.ToMicroNode.nodeRef + microLink.FromMicroNode.nodeRef);
	
	return microLinkQuery.FirstOrDefault();
	//select new 
	//{
	//	id = IdManager.computeUuid5<PositioningNetElement>(microLink.ToMicroNode.nodeRef + microLink.FromMicroNode.nodeRef),
	//	puic1 = microLink.ToMicroNode.nodeRef,
	//	puic2 = microLink.FromMicroNode.nodeRef,
	//};
	//
	//foreach (var x in microLinkQuery.ToList())
	//{
	//	if (x.puic1 == puic || x.puic2 == puic) return x.id;
	//}
	//
	//throw new Exception("Not Found");
}

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

	// create EULYNX bufferstop
	var bufferstopquery =
	from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
	where bufferstop is ProRail.IMSpoor.Model.BufferStop
	//select new VehicleStop()
	//{
	//	id = IdManager.computeUuid5<VehicleStop>(bufferstop.puic),
	//	locations = { new tElementWithIDref() { @ref = IdManager.computeUuid5<SpotLocation>(bufferstop.puic) } }
	//}
	select new Models.TopoModels.EULYNX.sig.BufferStop()
	{
		isOfBufferStopType = BufferStopTypes.fixated,
		id = bufferstop.puic,
		refersToRsmVehicleStop = new tElementWithIDref() { @ref = IdManager.computeUuid5<Models.TopoModels.EULYNX.rsmTrack.VehicleStop>(bufferstop.puic) },
	} as TrackAsset;
	
	dataContainer.ownsDataPrepEntities.ownsTrackAsset.AddRange(bufferstopquery.ToList());

	//bufferstopquery.Dump();

	// create RSM bufferstop
	var rsmVehicleStopQuery =
	from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
	where bufferstop is ProRail.IMSpoor.Model.BufferStop
	select new Models.TopoModels.EULYNX.rsmTrack.VehicleStop()
	{
		id = IdManager.computeUuid5<Models.TopoModels.EULYNX.rsmTrack.VehicleStop>(bufferstop.puic),
		locations = { new tElementWithIDref() { @ref = IdManager.computeUuid5<SpotLocation>(bufferstop.puic) } },
		name = bufferstop.name,
	};
	
	dataContainer.ownsRsmEntities.ownsVehicleStop.AddRange(rsmVehicleStopQuery.ToList());

	//rsmVehicleStopQuery.Dump();
	// create spotlocations of bufferstop
	var vehiclestopSpotLocationsQuery =
	from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
	where bufferstop is ProRail.IMSpoor.Model.BufferStop
	select new SpotLocation()
	{
		id = IdManager.computeUuid5<SpotLocation>(bufferstop.puic),
		coordinates = { new tElementWithIDref() { @ref = IdManager.computeUuid5<GeographicCoordinate>(bufferstop.puic) } },
		associatedNetElements = { 
			new AssociatedNetElement() {
				netElement = new tElementWithIDref(getNetElementIDForBufferstop(bufferstop.puic)),
			},
		},
	} as BaseLocation;
	
	dataContainer.ownsRsmEntities.usesLocation.AddRange(vehiclestopSpotLocationsQuery.ToList());

	vehiclestopSpotLocationsQuery.Dump();

	// create reference system coordinates
	var vehiclestopSpotLocationCoordinatesQuery =
	from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
	where bufferstop is ProRail.IMSpoor.Model.BufferStop
	let imspoorCoordinates = bufferstop.Location.GeographicLocation.Point.coordinates.Split(',')
	select new GeographicCoordinate()
	{
		id = IdManager.computeUuid5<GeographicCoordinate>(bufferstop.puic),

		// RD coordinaten, dutch geosystem
		longitude = Double.Parse(imspoorCoordinates[0].Replace(".", ",")), // 0 - 300   // Replace is nessecary because the point char will not be translated to 0.1, but the comma is
		latitude = Double.Parse(imspoorCoordinates[1].Replace(".", ",")), // 300 - 600 // see above ^
		elevation = Double.Parse(imspoorCoordinates[2].Replace(".", ",")), // ? NAP?   // see above ^

		positioningSystem = new tElementWithIDref() { @ref = IdManager.computeUuid5<PositioningSystem>("EPSG:28992") },
	} as PositioningSystemCoordinate;

	dataContainer.ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.AddRange(vehiclestopSpotLocationCoordinatesQuery.ToList());
	
	//vehiclestopSpotLocationCoordinatesQuery.Dump();
}
