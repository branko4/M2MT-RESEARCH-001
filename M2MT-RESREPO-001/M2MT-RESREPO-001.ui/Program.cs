using M2MT_RESREPO_001.Services;
using M2MT_RESREPO_001.TranslationManagers;

namespace M2MT_RESREPO_001.UI
{
    internal class Program
    {
        public static readonly string CURRENT_WORKING_DIRECTORY = Environment.GetEnvironmentVariable("researchPath", EnvironmentVariableTarget.Machine);

        static void Main(string[] args)
        {
            var dir = $"{CURRENT_WORKING_DIRECTORY}\\example-xml\\";

            var imspoorFileName = "Dordrecht_Merged_XSD_Validated_v2.xml";
            //var eulynxSaveFileName = "AutomaticTranslatedEULYNX.xml";
            var eulynxSaveFileName = "output-Jurjen.xml";

            // load XML
            var imspoor = IMSpoorReader.GetIMSpoor(Path.Combine(dir, imspoorFileName));

            if (imspoor == null) throw new NullReferenceException();

            // call translator
            var eulynx = new IMSpoorToEULYNXTranslationManager().GetEulynx(imspoor);

            // TODO save to file
            EulynxWriter.SaveEulynx(eulynx, Path.Combine(dir, eulynxSaveFileName));
        }
    }
}