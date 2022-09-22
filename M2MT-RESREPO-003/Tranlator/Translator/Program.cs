using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.rsmCommon;
using Models.TopoModels.EULYNX.sig;
using ProRail.IMSpoor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Translator.Util;

namespace Translator
{
	public class Program
    {
		public static readonly string CURRENT_WORKING_DIRECTORY = Environment.GetEnvironmentVariable("researchPath", EnvironmentVariableTarget.Machine);

		static void Main(string[] args)
        {
			// setup
			var eulynx = new EulynxDataPrepInterface();
			var dataContainer = getEmptyDataContainer();
			var situation = GetIMSpoor();
			var type = typeof(ITranslator);
			var translationTypes = typeof(Program).Assembly
				.GetTypes()
				.Where(typeX => type.IsAssignableFrom(typeX))
				.Where(typeX => !type.Equals(typeX))
				.ToList();
			ComplexIdManager.Init(situation);

            foreach (var translationType in translationTypes)
            {
                var trans = (ITranslator) Activator.CreateInstance(translationType);
                trans.Translate(situation, eulynx, dataContainer);
            }

			// finish
			eulynx.hasDataContainer = new List<DataContainer> { dataContainer };
			WriteToFile(eulynx);

			Console.WriteLine("Done");
        }

		static DataContainer getEmptyDataContainer()
		{
			return new DataContainer()
			{
				ownsDataPrepEntities = new DataPrepEntities()
				{
					ownsTrackAsset = new List<TrackAsset>(),
				},
				ownsRsmEntities = new RsmEntities()
				{
					ownsVehicleStop = new List<Models.TopoModels.EULYNX.rsmTrack.VehicleStop>(),
					usesTopography = new Topography()
					{
						usesPositioningSystem = new List<PositioningSystem>(),
						usesPositioningSystemCoordinate = new List<PositioningSystemCoordinate>(),
					},
					usesTrackTopology = new Topology()
					{
						usesNetElement = new List<PositioningNetElement>(),
						usesPositionedRelation = new List<PositionedRelation>(),
                    },
					usesLocation = new List<BaseLocation>(),
				},
			};
		}

		public static tSituation GetIMSpoor()
		{
            string path = $"{CURRENT_WORKING_DIRECTORY}\\example-xml\\Dordrecht_Merged_XSD_Validated.xml";
            XmlDocument doc = new XmlDocument();
			doc.Load(path);

			string xml = (string)XDocument.Load(path).ToString();

			var serializer = new XmlSerializer(typeof(IMSpoor));
			using (var tr = new StringReader(xml))
			{
				var imspoor = (IMSpoor)serializer.Deserialize(tr);
				var item = imspoor.Item;

				tSituation situation;
				if (item is tSituation)
				{
					situation = (tSituation)item;
				}
				else
				{
					Project project = (Project)item;
					situation = project.InitialSituation;
				}

				return situation;
			}
		}

		public static void WriteToFile(EulynxDataPrepInterface eulynx)
		{
			Type[] types = new Type[] { typeof(DataContainer) };
			XmlSerializer x = new XmlSerializer(typeof(EulynxDataPrepInterface));

			using (FileStream fs = File.Create($"{CURRENT_WORKING_DIRECTORY}\\example-xml\\by-linq-cs-code.xml"))
			{
				XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);

				writer.Formatting = Formatting.Indented;
				writer.Indentation = 4;

				x.Serialize(writer, eulynx);
			}
		}
	}
}
