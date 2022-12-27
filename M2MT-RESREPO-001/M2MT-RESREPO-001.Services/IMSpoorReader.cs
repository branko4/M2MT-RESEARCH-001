using ProRail.IMSpoor.Model;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace M2MT_RESREPO_001.Services
{
    public class IMSpoorReader
    {
        public static IMSpoor? GetIMSpoor(string path)
        {
            XmlDocument doc = new XmlDocument();
            
            doc.Load(path);

            string xml = (string) XDocument.Load(path).ToString();
            
            var serializer = new XmlSerializer(typeof(IMSpoor));
            using (var tr = new StringReader(xml))
            {
                return (IMSpoor?) serializer.Deserialize(tr);
            }
        }
    }
}
