using M2MT_RESREPO_001.Services;
using Models.TopoModels.EULYNX.rsmCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.Topology
{
    internal class PositioningNetElementsTranslationManager
    {
        private static readonly PositioningNetElementsTranslationManager positioningNetElementsTranslationManager = new PositioningNetElementsTranslationManager();
        public static PositioningNetElementsTranslationManager GetInstance()
        {
            return positioningNetElementsTranslationManager;
        }

        private Dictionary<string, List<Guid>> puicWithIds = new Dictionary<string, List<Guid>>();

        private PositioningNetElementsTranslationManager() { }

        internal List<PositioningNetElement> GetNetElements()
        {
            var iMSpoorProviderService = IMSpoorProviderService.GetInstance();
            var situation = iMSpoorProviderService.Situation;
            var netElements = new List<PositioningNetElement>();
            foreach (var microLink in situation.RailInfrastructure.RailTopology.MicroLinks)
            {
                Guid guid = Guid.NewGuid();
                netElements.Add(
                    new PositioningNetElement()
                    {
                        id = guid.ToString(),
                        //id = IdManager.computeUuid5<PositioningNetElement>(microLink.ToMicroNode.nodeRef + microLink.FromMicroNode.nodeRef),
                    }
                );

                saveToDB(microLink.ToMicroNode.nodeRef, microLink.FromMicroNode.nodeRef, guid);
                
            }

            return netElements;
        }

        private void saveToDB(string nodeRef1, string nodeRef2, Guid guid)
        {
            saveToDB(nodeRef1, guid);
            saveToDB(nodeRef2, guid);
        }

        private void saveToDB(string puic, Guid guid)
        {
            if (!puicWithIds.ContainsKey(puic)) puicWithIds[puic] = new List<Guid>();
            if (puicWithIds[puic].Contains(guid)) return;

            puicWithIds[puic].Add(guid);
        }

        public Guid GetGuidByPuic(string puic)
        {
            return puicWithIds[puic].FirstOrDefault();
        }
    }
}
