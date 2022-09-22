using M2MT_RESREPO_001.Services;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.LocationTranslationManagers;
using Models.TopoModels.EULYNX.rsmCommon;
using Models.TopoModels.EULYNX.rsmTrack;
using ProRail.IMSpoor.Model;
using System;

namespace M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers
{
    internal class VehicleStopTranslationManager
    {
        private static VehicleStopTranslationManager _vehicleStopTranslationManager = new VehicleStopTranslationManager();
        public static VehicleStopTranslationManager GetInstance() { return _vehicleStopTranslationManager; }
        private VehicleStopTranslationManager() { }

        internal Dictionary<string, string> PUICWithVehicleStopsID = new Dictionary<string, string>();
        internal List<VehicleStop> GetVehicleStops()
        {
            var iMSpoorProviderService = IMSpoorProviderService.GetInstance();
            var junctions = iMSpoorProviderService.Situation.RailInfrastructure.RailImplementation.Junctions;

            var bufferstops = new List<BufferStop>();
            var vehicleStops = new List<VehicleStop>();

            var spotLocationTranslationManager = SpotLocationTranslationManager.GetInstance();

            // filter on bufferstops
            foreach (var item in junctions)
            {
                if (item.GetType() == typeof(BufferStop))
                {
                    bufferstops.Add((BufferStop)item);

                    // get all vechicolStopLocations from IMSpoor
                    var vehicleStop = new VehicleStop();
                    // TODO calculete good value
                    vehicleStop.id = Guid.NewGuid().ToString();

                    vehicleStop.locations = new List<tElementWithIDref>();
                    vehicleStop.locations.Add(spotLocationTranslationManager.CreateSpotLocation(item.Location.GeographicLocation, item.puic));

                    vehicleStop.name = item.name;

                    PUICWithVehicleStopsID.Add(item.puic, vehicleStop.id);
                    vehicleStops.Add(vehicleStop);
                }
            }

            // return all stoplocations
            return vehicleStops;
        }

        internal tElementWithIDref? getReference(BufferStop imspoorBufferstop)
        {
            var id = this.PUICWithVehicleStopsID[imspoorBufferstop.puic];
            var element = new tElementWithIDref();
            element.@ref = id;
            return element;
        }
    }
}
