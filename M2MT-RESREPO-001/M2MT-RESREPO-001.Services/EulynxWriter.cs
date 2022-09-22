using Models.TopoModels.EULYNX.generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace M2MT_RESREPO_001.Services
{
    public class EulynxWriter
    {
        public static void SaveEulynx(EulynxDataPrepInterface eulynx, string path)
        {
            Type[] types = new Type[] { typeof(DataContainer) };
            XmlSerializer x = new XmlSerializer(typeof(EulynxDataPrepInterface));

            using (FileStream fs = File.Create(path))
            {
                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);

                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                x.Serialize(writer, eulynx);
            }
        }
    }
}