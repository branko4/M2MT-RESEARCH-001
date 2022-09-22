using Models.TopoModels.EULYNX.generic;
using ProRail.IMSpoor.Model;

namespace Translator
{
    // This interface can be used to add functions
    interface ITranslator
    {
        public void Translate(tSituation situation, EulynxDataPrepInterface eulynx, DataContainer dataContainer);
    }
}
