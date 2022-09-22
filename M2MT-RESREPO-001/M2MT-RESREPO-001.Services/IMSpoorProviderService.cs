using ProRail.IMSpoor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M2MT_RESREPO_001.Services
{
    public class IMSpoorProviderService
    {
        private static IMSpoorProviderService? iMSpoorProviderService;
        public static IMSpoorProviderService GetInstance()
        {
            if (iMSpoorProviderService == null) iMSpoorProviderService = new IMSpoorProviderService();
            return iMSpoorProviderService;
        }

        public tSituation? Situation { get; set; }

        private IMSpoorProviderService() { }
    }
}
