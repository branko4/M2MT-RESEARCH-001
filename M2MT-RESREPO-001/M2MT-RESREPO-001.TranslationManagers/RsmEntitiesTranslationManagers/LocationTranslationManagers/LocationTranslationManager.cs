using Models.TopoModels.EULYNX.rsmCommon;

namespace M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.LocationTranslationManagers
{
    internal class LocationTranslationManager
    {

        internal List<BaseLocation> GetLocations()
        {
            // Create ListForLocations
            var locations = new List<BaseLocation>();

            // get SpotLocations
            var spotLocationTranslationManager = SpotLocationTranslationManager.GetInstance();
            locations.AddRange(spotLocationTranslationManager.GetSpotLocations());

            // TODO get LinearLocations
            // TODO get AreaLocations

            return locations;
        }
    }
}