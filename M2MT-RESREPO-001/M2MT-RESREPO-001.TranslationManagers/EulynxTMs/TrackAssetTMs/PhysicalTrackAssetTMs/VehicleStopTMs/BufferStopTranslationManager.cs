using M2MT_RESREPO_001.Services;
using Models.TopoModels.EULYNX.sig;
using ProRail.IMSpoor.Model;
using IMSpoorBufferStop = ProRail.IMSpoor.Model.BufferStop;
using EULYNXBufferStop = Models.TopoModels.EULYNX.sig.BufferStop;
using RSMVehicleStopTranslationManager = M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.VehicleStopTranslationManager;

namespace M2MT_RESREPO_001.TranslationManagers.EulynxTMs.TrackAssetTMs.PhysicalTrackAssetTMs.VehicleStopTMs
{
    internal class BufferStopTranslationManager
    {
        internal IEnumerable<EULYNXBufferStop> getBufferStops()
        {
            // get IMSpoor situation
            var iMSpoorProviderService = IMSpoorProviderService.GetInstance();
            var situation = iMSpoorProviderService.Situation;

            // get RSM vehicleStop translation manager
            var rsmVehicleStopTranslationManager = RSMVehicleStopTranslationManager.GetInstance();

            // create bufferstops list
            var bufferStops = new List<EULYNXBufferStop>();

            // get IMSpoor bufferstop
            //   get IMSpoor junctions
            var junctions = situation.RailInfrastructure.RailImplementation.Junctions;

            // filter imspoor junctions to imspoor bufferstops
            foreach (var junction in junctions)
            {
                if (junction.GetType() == typeof(IMSpoorBufferStop))
                {
                    // cast imspoor junction to imspoor bufferstop
                    var imspoorBufferstop = (IMSpoorBufferStop)junction;

                    // create bufferstop
                    var eulynxBufferstop = new EULYNXBufferStop();
                    eulynxBufferstop.id = imspoorBufferstop.puic;
                    eulynxBufferstop.isOfBufferStopType = this.TranslateBufferstopType(imspoorBufferstop.bufferstopType);
                    
                    //   get correct bufferstop type
                    //eulynxBufferstop.isOfBufferStopType = BufferStopTypes.fixated;
                    eulynxBufferstop.refersToRsmVehicleStop = rsmVehicleStopTranslationManager.getReference(imspoorBufferstop);

                    //   TODO get other relevant elements of bufferstop

                    // add bufferstop to bufferstops list
                    bufferStops.Add(eulynxBufferstop);
                }
            }

            // return bufferstops list
            return bufferStops;
        }

        private BufferStopTypes TranslateBufferstopType(BufferstopType imspoortype)
        {
            switch(imspoortype)
            {
                case (BufferstopType.hydraulic): return BufferStopTypes.hydraulic;
                case (BufferstopType.friction): return BufferStopTypes.friction;
                case (BufferstopType.fixated): return BufferStopTypes.fixated;
                case (BufferstopType.other): return BufferStopTypes.other;
                default:
                    throw new Exception("Invallid data");
            }
        }
    }
}
