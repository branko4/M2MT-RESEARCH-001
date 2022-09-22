using ProRail.IMSpoor.Model;
using Models.TopoModels.EULYNX.generic;
using M2MT_RESREPO_001.Services;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers;
using M2MT_RESREPO_001.TranslationManagers.EulynxTranslationManagers;
using M2MT_RESREPO_001.Services.TranslationServices;
using Models.TopoModels.EULYNX.rsmCommon;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.LocationTranslationManagers;

namespace M2MT_RESREPO_001.TranslationManagers
{
    public class IMSpoorToEULYNXTranslationManager
    {
        public EulynxDataPrepInterface GetEulynx(IMSpoor imSpoor)
        {
            object item = imSpoor.Item;

            if (item == null) return null;
            // TODO get IMX version

            // get situation && save
            //tSituation situation;
            var imspoorProviderService = IMSpoorProviderService.GetInstance();
            if (item is tSituation)
            {
                imspoorProviderService.Situation = (tSituation)item;
            }
            else
            {
                Project project = (Project)item;
                imspoorProviderService.Situation = project.InitialSituation;
            }


            // get RSM entities
            var rsmEntitiesTranslationManager = new RSMEntitiesTranslationManager();
            var rsmEntities = rsmEntitiesTranslationManager.GetEntities();

            // get Eulynx Entities
            var eulynxTranslationManager = new EulynxTranslationManager();
            var dataPrepEntities = eulynxTranslationManager.GetDataPrepEntities();

            // TODO link RSM and EULYNX entities
            rsmEntities.usesTopography = new Topography();
            rsmEntities.usesTopography.usesPositioningSystem = PositioningSystemTranslationService.GetInstance().GetPositioningSystems();
            rsmEntities.usesTopography.usesPositioningSystemCoordinate = new List<PositioningSystemCoordinate>();
            rsmEntities.usesTopography.usesPositioningSystemCoordinate.AddRange(GeographicCoordinateTranslationService.GetInstance().GetGeographicCoordinates());

            rsmEntities.usesLocation.AddRange(SpotLocationTranslationManager.GetInstance().GetRestOfSpotLocations());

            // create container
            var eulynx = new EulynxDataPrepInterface();
            var dataprepContainer = new DataContainer();
            eulynx.hasDataContainer = new List<DataContainer>();

            // save RSM
            dataprepContainer.ownsRsmEntities = rsmEntities;

            // save Eulynx
            dataprepContainer.ownsDataPrepEntities = dataPrepEntities;

            eulynx.hasDataContainer.Add(dataprepContainer);

            return eulynx;
        }
    }
}
