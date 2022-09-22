using Models.TopoModels.EULYNX.sig;

namespace M2MT_RESREPO_001.TranslationManagers.EulynxTMs.TrackAssetTMs.PhysicalTrackAssetTMs.VehicleStopTMs
{
    internal class VehicleStopTranslationManager
    {

        internal IEnumerable<VehicleStop> getVehicleStops()
        {
            // create VehicleStops List
            var vehicleStops = new List<VehicleStop>();

            // get bufferstops
            var bufferStopTranslationManager = new BufferStopTranslationManager();
            vehicleStops.AddRange(bufferStopTranslationManager.getBufferStops());

            // TODO get other vehicleStops

            // return vehicleStops list
            return vehicleStops;
        }
    }
}