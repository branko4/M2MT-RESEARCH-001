using M2MT_RESREPO_001.Services;
using M2MT_RESREPO_001.Services.TranslationServices;
using M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.LocationTranslationManagers;
using Models.TopoModels.EULYNX.rsmCommon;
using Models.TopoModels.EULYNX.sig;
using IMSpoorBufferStop = ProRail.IMSpoor.Model.BufferStop;
using RSMVehicleStopTranslationManager = M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.VehicleStopTranslationManager;

namespace M2MT_RESREPO_001.TranslationManagers.EulynxTMs.TrackAssetTMs.PhysicalTrackAssetTMs.VehicleStopTMs
{
    internal class BufferStopTranslationManager
    {
        internal IEnumerable<BufferStop> getBufferStops()
        {
            // get IMSpoor situation
            var iMSpoorProviderService = IMSpoorProviderService.GetInstance();
            var situation = iMSpoorProviderService.Situation;

            // get RSM vehicleStop translation manager
            var rsmVehicleStopTranslationManager = RSMVehicleStopTranslationManager.GetInstance();

            // create bufferstops list
            var bufferStops = new List<BufferStop>();

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
                    var eulynxBufferstop = new BufferStop();
                    eulynxBufferstop.id = imspoorBufferstop.puic;
                    
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
    }
}
