using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.rsmCommon;
using ProRail.IMSpoor.Model;

namespace M2MT_RESREPO_001.Services.TranslationServices
{
    public class GeographicCoordinateTranslationService
    {
        private static GeographicCoordinateTranslationService Instance = new GeographicCoordinateTranslationService();
        public static GeographicCoordinateTranslationService GetInstance()
        {
            return Instance;
        }

        private EulynxDataPrepInterface eulynx;
        private PositioningSystemTranslationService positioningSystemTranslationService;
        private Dictionary<string, GeographicCoordinate> cordinateWithClass = new Dictionary<string, GeographicCoordinate>();

        private GeographicCoordinateTranslationService() {
            positioningSystemTranslationService = PositioningSystemTranslationService.GetInstance();
        }

        public void SetEULYNX(EulynxDataPrepInterface eulynx)
        {
            this.eulynx = eulynx;
        }

        public tElementWithIDref CreateForRDaGeographicCoordinate(tPoint geographicLocation)
        {
            GeographicCoordinate cordinate;
            try
            {
                cordinate = cordinateWithClass[geographicLocation.Point.coordinates];
            } 
            catch (KeyNotFoundException e)
            {
                cordinate = new GeographicCoordinate();
                //var imspoorCoordinates = imspoorBufferstop.Location.GeographicLocation.Point.coordinates.Split(',');
                var imspoorCoordinates = geographicLocation.Point.coordinates.Split(',');
                // RD coordinaten, dutch geosystem
                cordinate.longitude = Double.Parse(imspoorCoordinates[0].Replace(".",",")); // 0 - 300   // Replace is nessecary because the point char will not be translated to 0.1, but the comma is
                cordinate.latitude = Double.Parse(imspoorCoordinates[1].Replace(".", ",")); // 300 - 600 // see above ^
                cordinate.elevation = Double.Parse(imspoorCoordinates[2].Replace(".", ",")); // ? NAP?   // see above ^
                cordinate.id = Guid.NewGuid().ToString();

                cordinate.positioningSystem = positioningSystemTranslationService.GetPositioningSystemReference(geographicLocation.Point.srsName);

                cordinateWithClass[geographicLocation.Point.coordinates] = cordinate;
            }
            var reference = new tElementWithIDref();
            reference.@ref = cordinate.id;
            return reference;
        }

        public List<GeographicCoordinate> GetGeographicCoordinates()
        {
            return cordinateWithClass.Values.ToList();
        }
    }
}
