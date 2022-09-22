using M2MT_RESREPO_001.Services;
using M2MT_RESREPO_001.Services.TranslationServices;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.Topology;
using Models.TopoModels.EULYNX.rsmCommon;
using ProRail.IMSpoor.Model;

namespace M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.LocationTranslationManagers
{
    internal class SpotLocationTranslationManager
    {
        private static SpotLocationTranslationManager instance = new SpotLocationTranslationManager();

        public static SpotLocationTranslationManager GetInstance()
        {
            return instance;
        }

        private Dictionary<string, SpotLocation> coordinateWithSpotLocation = new Dictionary<string, SpotLocation>();
        private GeographicCoordinateTranslationService _geographicCoordinateTranslationService;

        private SpotLocationTranslationManager() {
            _geographicCoordinateTranslationService = GeographicCoordinateTranslationService.GetInstance();
        }

        internal IEnumerable<SpotLocation> GetSpotLocations()
        {
            var iMSpoorProviderService = IMSpoorProviderService.GetInstance();
            var situation = iMSpoorProviderService.Situation;
            // TODO FIXME find all spotLocations from IMSpoor?

            // TODO find all relevant spotLocations from IMSpoor for a bufferstop
            var spotLocations = new List<SpotLocation>();

            // TODO choose between create spot location when needed and add or read all spot locations at once
            

            // return not null value
            return spotLocations;
        }

        internal tElementWithIDref CreateSpotLocation(tPoint geographicLocation, string puic)
        {
            var spotLocation = new SpotLocation();
            spotLocation.id = Guid.NewGuid().ToString();
            spotLocation.coordinates = new List<tElementWithIDref>();

            var coordinate = _geographicCoordinateTranslationService.CreateForRDaGeographicCoordinate(geographicLocation);
            spotLocation.coordinates.Add(coordinate);

            coordinateWithSpotLocation.Add(geographicLocation.Point.coordinates, spotLocation);

            spotLocation.associatedNetElements = new List<AssociatedNetElement>{ 
                new AssociatedNetElement()
                {
                    netElement = new tElementWithIDref() { @ref= PositioningNetElementsTranslationManager.GetInstance().GetGuidByPuic(puic).ToString() },
                },
            };

            var reference = new tElementWithIDref();
            reference.@ref = spotLocation.id;
            return reference;
        }

        internal IEnumerable<BaseLocation> GetRestOfSpotLocations()
        {
            return coordinateWithSpotLocation.Values.ToList();
        }
    }
}