using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.rsmCommon;

namespace M2MT_RESREPO_001.Services.TranslationServices
{
    public class PositioningSystemTranslationService
    {
        private static PositioningSystemTranslationService Instance = new PositioningSystemTranslationService();
        public static PositioningSystemTranslationService GetInstance()
        {
            return Instance;
        }

        private EulynxDataPrepInterface eulynx;
        private Dictionary<string, PositioningSystem> positioningSystemNameWithClass = new Dictionary<string, PositioningSystem>();

        private PositioningSystemTranslationService() { }

        public void SetEULYNX(EulynxDataPrepInterface eulynx)
        {
            this.eulynx = eulynx;
        }

        public tElementWithIDref GetPositioningSystemReference(string name)
        {
            PositioningSystem positioningSystem;
            try
            {
                positioningSystem = positioningSystemNameWithClass[name];
            }
            catch (KeyNotFoundException e)
            {
                positioningSystem = new PositioningSystem();
                positioningSystem.name = name;
                positioningSystem.id = Guid.NewGuid().ToString();

                positioningSystemNameWithClass[name] = positioningSystem;
            }

            //if (positioningSystem == null)
            //{
            //}

            var reference = new tElementWithIDref();
            reference.@ref = positioningSystem.id;
            return reference;
        }

        public List<PositioningSystem> GetPositioningSystems()
        {
            return positioningSystemNameWithClass.Values.ToList();
        }
    }
}
