using Models.TopoModels.EULYNX.generic;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.LocationTranslationManagers;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.Topology;

namespace M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers
{
    internal class RSMEntitiesTranslationManager
    {
        internal RsmEntities GetEntities()
        {
            // create rsmEntities object
            var rsmEntities = new RsmEntities();

            // Get track topology // WARNING: do not move method bellow the get location, since the GetPositionedRelations should be called before the elements references are made
            rsmEntities.usesTrackTopology = TopologyTranslationManager.GetTrackTopologyInstance().GetTopology();

            // get locations
            var locationTranslationManager = new LocationTranslationManager();
            rsmEntities.usesLocation = locationTranslationManager.GetLocations();

            // get vehicleStops
            var vehicleStopTranslationManager = VehicleStopTranslationManager.GetInstance();
            rsmEntities.ownsVehicleStop = vehicleStopTranslationManager.GetVehicleStops();


            return rsmEntities;
        }
    }
}
