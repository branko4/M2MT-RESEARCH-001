<Query Kind="Program" />

public static readonly string CURRENT_WORKING_DIRECTORY = Environment.GetEnvironmentVariable("researchPath", EnvironmentVariableTarget.Machine);

public class CodeWriter
{
	public static void WriteToFile(string code, string OBJECT_NAME)
	{
		File.WriteAllTextAsync($"{CURRENT_WORKING_DIRECTORY}\\M2MT-RESREPO-003\\Tranlator\\Translator\\{OBJECT_NAME}.cs", code);
	}

}

void Main()
{
	List<string> objects = new List<string>{
	 "BufferStop",
	 "PositioningSystem",
	 "TrackTopology",
	};
	
	var packageManager = new PackageManager();
	
	foreach (var objectName in objects)
	{
		CreateCodeFile(objectName, packageManager);
	}
}

void CreateCodeFile(string OBJECT_NAME, PackageManager packageManager)
{
	string path = $"{CURRENT_WORKING_DIRECTORY}\\M2MT-RESREPO-003\\{OBJECT_NAME}.linq";

	var fileContent = File.ReadAllText(path);
//	using (FileStream fileStream = File.Open(path, FileMode.Open))
//	{
//		byte[] b = new byte[1024];
//		UTF8Encoding temp = new UTF8Encoding(true);
//
//		var fileContent = "";
//
//		while (fileStream.Read(b, 0, b.Length) > 0)
//		{
//			Console.WriteLine(temp.GetString(b));
//			fileContent += temp.GetString(b);
//		}
//		Console.WriteLine("\n\n");
	var lines = fileContent.Split('\n').ToList();
	lines.Dump();
	var index = 0;
	var startIndex = 0;
	var usingStatements = new List<string>();
	var codeLines = new List<string>();
	lines.Count.Dump();
	foreach (var line in lines)
	{
		if (line.Contains("void Translate("))
		{
			codeLines = lines.GetRange(index, (lines.Count - index));
			//codeLines.Dump();
		}
		if (line.Contains("<Query"))
		{
			startIndex = index + 1;
		}
		if (line.Contains("</Query>"))
		{
			usingStatements = lines.GetRange(startIndex, index);
		}
		index++;
	}
	string codeFileContent = "";
	codeFileContent += $"using System.Linq;\n";
	codeFileContent += $"using Translator.Util;\n";
	foreach (var usingStatement in usingStatements)
	{
		if (usingStatement.Contains("<Namespace>"))
		{
			var startOfIndex = usingStatement.IndexOf('>') + 1;
			var endOfIndex = usingStatement.IndexOf('<', startOfIndex);
			var cleanedStatement = usingStatement.Substring(startOfIndex, endOfIndex - startOfIndex);
			codeFileContent += $"using {cleanedStatement};\n";
		}
		else if (usingStatement.Contains("<NuGetReference>"))
		{
			var startOfIndex = usingStatement.IndexOf('>') + 1;
			var endOfIndex = usingStatement.IndexOf('<', startOfIndex);
			var cleanedStatement = usingStatement.Substring(startOfIndex, endOfIndex - startOfIndex);
			
			if (!packageManager.packages.Contains(cleanedStatement)) {
				packageManager.packages.Add(cleanedStatement);
			}
		}
	}

	const char START_TAG = '{';
	const char END_TAG = '}';
	var _prefix = "\t";

	codeFileContent += "\nnamespace Translator \n{\n";
	//_prefix = "\t";

	codeFileContent += $"{_prefix}public class M2MT{OBJECT_NAME} : ITranslator\n{_prefix}{START_TAG}\n";

	//_prefix += "\t\t";

	foreach (var readCodeLine in codeLines)
	{
		var codeLine = readCodeLine;
		if (codeLine.Contains("Dump") || codeLine.Contains("//")) continue;
		if (codeLine.Contains("getNetElementIDForBufferstop(bufferstop.puic)")) codeLine = codeLine.Replace("getNetElementIDForBufferstop(bufferstop.puic)","ComplexIdManager.getNetElementIDForBufferstop(bufferstop.puic)");
		if (codeLine.Contains("void Translate("))
		{
			codeFileContent += $"{_prefix}{_prefix}public {codeLine}\n";
			continue;
		}
		codeFileContent += $"{_prefix}{_prefix}{codeLine}\n";
	}

	//_prefix = "\t";
	codeFileContent += $"{_prefix}{END_TAG}\n{END_TAG}";

	//codeFileContent.Replace("\t", "--").Split('\n').Dump();

	CodeWriter.WriteToFile(codeFileContent, OBJECT_NAME);
	//}
}

public class PackageManager {
	public List<string> packages = new List<string>();
	
	public void GetPackages() {
		foreach (var package in packages)
		{
			
		}
	}
}