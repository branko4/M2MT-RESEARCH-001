using M2MT_RESREPO_001.TranslationManagers.EulynxTranslationManagers.TrackAssetTranslationManagers.PhysicalTrackAssetTranslationManagers;
using Models.TopoModels.EULYNX.sig;

namespace M2MT_RESREPO_001.TranslationManagers.EulynxTranslationManagers.TrackAssetTranslationManagers
{
    internal class TrackAssetTranslationManager
    {
        internal List<TrackAsset> GetTrackAssets()
        {
            // create trackAssets list
            var trackAssets = new List<TrackAsset>();

            // get physical track assets with bufferstop
            var physicalTrackAssetTranslationManager = new PhysicalTrackAssetTranslationManager();
            trackAssets.AddRange(physicalTrackAssetTranslationManager.GetPhysicalTrackAssets());

            // TODO add other track asset types

            // return track assets
            return trackAssets;
        }
    }
}