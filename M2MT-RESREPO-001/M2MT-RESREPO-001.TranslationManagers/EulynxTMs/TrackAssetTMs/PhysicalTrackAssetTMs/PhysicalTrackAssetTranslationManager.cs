using M2MT_RESREPO_001.TranslationManagers.EulynxTMs.TrackAssetTMs.PhysicalTrackAssetTMs.VehicleStopTMs;
using Models.TopoModels.EULYNX.sig;

namespace M2MT_RESREPO_001.TranslationManagers.EulynxTranslationManagers.TrackAssetTranslationManagers.PhysicalTrackAssetTranslationManagers
{
    internal class PhysicalTrackAssetTranslationManager
    {
        internal IEnumerable<PhysicalTrackAsset> GetPhysicalTrackAssets()
        {
            // create PhysicalTrackAsset list
            var physicalTrackAssets = new List<PhysicalTrackAsset>();

            // add vehicleStops with bufferstop
            var vehicleStopTranslationManager = new VehicleStopTranslationManager();
            physicalTrackAssets.AddRange(vehicleStopTranslationManager.getVehicleStops());

            // TODO add other physicalTrackAssets

            // return physicalTrackAssets
            return physicalTrackAssets;
        }
    }
}