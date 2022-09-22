using M2MT_RESREPO_001.TranslationManagers.EulynxTranslationManagers.TrackAssetTranslationManagers;
using Models.TopoModels.EULYNX.generic;

namespace M2MT_RESREPO_001.TranslationManagers.EulynxTranslationManagers
{
    internal class EulynxTranslationManager
    {
        internal DataPrepEntities GetDataPrepEntities()
        {
            // create DataPrepEntities
            var entities = new DataPrepEntities();

            // get trackassets with bufferstop
            var trackAssetTranslationManager = new TrackAssetTranslationManager();
            entities.ownsTrackAsset = trackAssetTranslationManager.GetTrackAssets();

            // TODO get ATP

            // TODO get ficitousSignal

            // return DataPrepEntites
            return entities;
        }
    }
}