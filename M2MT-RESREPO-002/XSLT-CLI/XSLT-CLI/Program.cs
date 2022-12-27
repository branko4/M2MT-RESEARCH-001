using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XSLT_CLI
{
    interface ArgumentManger
    {
        public void LoadArguments(string[] args);
        public string getArgument(string argumentName);

        public bool ArgumentIsSet(string argumentName);
    }

    class SimpleArgumentMAnager : ArgumentManger

    {
        private Dictionary<string, string> arguments;

        public bool ArgumentIsSet(string argumentName)
        {
            return this.arguments.ContainsKey(argumentName);
        }

        public string getArgument(string argumentName)
        {
            // FIXME use argumentIsSet methode
            if (!this.arguments.ContainsKey(argumentName)) throw new ArgumentException($"The {argumentName} argument is not assigned");
            var value = this.arguments[argumentName];
            if (String.IsNullOrEmpty(value)) throw new ArgumentException($"The {argumentName} arguement should have a value asigned");
            return value;
        }

        public void LoadArguments(string[] args)
        {
            if (args.Length % 2 == 1) throw new ArgumentException("invalid arguments");

            this.arguments = new Dictionary<string, string>();

            for (int argumentIndex = 0; argumentIndex < args.Length; argumentIndex++)
            {
                var argumentName = args[argumentIndex];
                argumentIndex++;
                var argumentValue = args[argumentIndex];

                if (argumentName == null) throw new ArgumentException("invalide argument name given");
                if (!argumentName.StartsWith("--")) throw new ArgumentException("invalide argument name given");

                if (argumentValue == null) throw new ArgumentException("invalide argument value given");

                this.arguments.Add(argumentName, argumentValue);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine($"--out \t\t; \t [optioneel] specifiy output file, if not given output will be console");
                Console.WriteLine($"--workdir\t; \t [optioneel] specifiy other working directory then current");
                Console.WriteLine($"--xml \t\t; \t specify xml file (location)");
                Console.WriteLine($"--xsl \t\t; \t specify xsl file (location)");
                return;
            }

            // load arguments
            ArgumentManger argumentManager = new SimpleArgumentMAnager();
            argumentManager.LoadArguments(args);

            var xsltArguments = new XsltArgumentList();
            xsltArguments.AddExtensionObject("M2MT:SDK", new SDK());

            Console.WriteLine("Strarting to translate");

            //Create a new XslTransform object.
            var xslt = new XslCompiledTransform();

            string workingDirectory = Directory.GetCurrentDirectory();

            if (argumentManager.ArgumentIsSet("--workdir")) workingDirectory = argumentManager.getArgument("--workdir");

            //Load the stylesheet.
            var pathToXSL = Path.Combine(workingDirectory, argumentManager.getArgument("--xsl"));
            Console.WriteLine($"Reading XSLT document: {pathToXSL}");
            xslt.Load(pathToXSL);

            //Create a new XPathDocument and load the XML data to be transformed.
            var pathToData = Path.Combine(workingDirectory, argumentManager.getArgument("--xml"));
            Console.WriteLine($"Reading XML document: {pathToData}");
            XPathDocument mydata = new XPathDocument(pathToData);

            // FIXME create option for output directory
            //Create an XmlTextWriter which outputs to the console.
            XmlWriter writer = new XmlTextWriter(Console.Out);
            if (argumentManager.ArgumentIsSet("--out"))
            {
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.ConformanceLevel = ConformanceLevel.Auto;
                var savePath = Path.Combine(workingDirectory, argumentManager.getArgument("--out"));
                Console.WriteLine($"Saving file to: {savePath}");
                writer = XmlWriter.Create(savePath, settings);
            }

            try
            {
                //Transform the data and send the output to the console.
                xslt.Transform(mydata, xsltArguments, writer, null);
            } finally
            {
                writer.Close();
            }
            
        }
    }
}
