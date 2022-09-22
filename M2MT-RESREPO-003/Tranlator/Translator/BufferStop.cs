using System.Linq;
using Translator.Util;
using ProRail.IMSpoor.Model;
using System.Xml.Serialization;
using Models.TopoModels.EULYNX.rsmCommon;
using Models.TopoModels.EULYNX.rsmTrack;
using Models.TopoModels.EULYNX.sig;
using Models.TopoModels.EULYNX.generic;
using Translator.Util;

namespace Translator 
{
	public class M2MTBufferStop : ITranslator
	{
		public void Translate(tSituation situation, EulynxDataPrepInterface eulynx, DataContainer dataContainer) 
		{
		
			var bufferstopquery =
			from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
			where bufferstop is ProRail.IMSpoor.Model.BufferStop
			select new Models.TopoModels.EULYNX.sig.BufferStop()
			{
				isOfBufferStopType = BufferStopTypes.fixated,
				id = bufferstop.puic,
				refersToRsmVehicleStop = new tElementWithIDref() { @ref = IdManager.computeUuid5<Models.TopoModels.EULYNX.rsmTrack.VehicleStop>(bufferstop.puic) },
			} as TrackAsset;
			
			dataContainer.ownsDataPrepEntities.ownsTrackAsset.AddRange(bufferstopquery.ToList());
		
		
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
		
			var vehiclestopSpotLocationsQuery =
			from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
			where bufferstop is ProRail.IMSpoor.Model.BufferStop
			select new SpotLocation()
			{
				id = IdManager.computeUuid5<SpotLocation>(bufferstop.puic),
				coordinates = { new tElementWithIDref() { @ref = IdManager.computeUuid5<GeographicCoordinate>(bufferstop.puic) } },
				associatedNetElements = { 
					new AssociatedNetElement() {
						netElement = new tElementWithIDref(ComplexIdManager.getNetElementIDForBufferstop(bufferstop.puic)),
					},
				},
			} as BaseLocation;
			
			dataContainer.ownsRsmEntities.usesLocation.AddRange(vehiclestopSpotLocationsQuery.ToList());
		
		
			var vehiclestopSpotLocationCoordinatesQuery =
			from bufferstop in situation.RailInfrastructure.RailImplementation.Junctions
			where bufferstop is ProRail.IMSpoor.Model.BufferStop
			let imspoorCoordinates = bufferstop.Location.GeographicLocation.Point.coordinates.Split(',')
			select new GeographicCoordinate()
			{
				id = IdManager.computeUuid5<GeographicCoordinate>(bufferstop.puic),
		
		
				positioningSystem = new tElementWithIDref() { @ref = IdManager.computeUuid5<PositioningSystem>("EPSG:28992") },
			} as PositioningSystemCoordinate;
		
			dataContainer.ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.AddRange(vehiclestopSpotLocationCoordinatesQuery.ToList());
			
		}
		
	}
}