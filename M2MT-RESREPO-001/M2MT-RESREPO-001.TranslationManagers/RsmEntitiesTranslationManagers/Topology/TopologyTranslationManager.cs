using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cTopology = Models.TopoModels.EULYNX.generic.Topology;

namespace M2MT_RESREPO_001.TranslationManagers.RsmEntitiesTranslationManagers.Topology
{
    internal class TopologyTranslationManager
    {
        private static readonly TopologyTranslationManager trackTopologyInstance = new TopologyTranslationManager();
        public static TopologyTranslationManager GetTrackTopologyInstance()
        {
            return trackTopologyInstance;
        }

        private TopologyTranslationManager() { }

        public cTopology GetTopology()
        {
            // init a topology object
            return new cTopology()
            {
                // get Position net element
                usesNetElement = PositioningNetElementsTranslationManager.GetInstance().GetNetElements(),
                // get position relation
                //usesPositionedRelation = PositionedRelationTranslationManager.GetInstance().GetPositionedRelations(),
            };

        }
    }
}
