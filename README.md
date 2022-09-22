# M2MT-RESEARCH-001
 Repository containing research on translation options.

## ⚠Important⚠
Al projects build further on the system environment variable "researchPath". For running this project correctly you should set the variable or you should replace all ```Environment.GetEnvironmentVariable("researchPath", EnvironmentVariableTarget.Machine);``` with your target path.
Note that the project directory located at `M2MT-RESREPO-003/Translator/Translator` should be reachable. So if your path is not the root of this repository you should also fix this in the `LINQPadPreCompiler.linq` file.

## Instructions

### M2MT-RESREPO-001
To translate a file run the UI class library.

### M2MT-RESREPO-002
To translate a file run:
```
XSLT-CLI.exe --xml <path-of-file-to-be-translated> --xsl <path-of-XSL-file> --workdir <prefix-for-all-paths> --out <path-of-output-file>
```

where
- '<path-of-file-to-be-translated>': The path and file name of the file that needs to be translated
- '<path-of-XSL-file>': The path and file name of the file that contains the XSL
- '<path-of-output-file>': The path and file name of the file where the translation result is accepted to be saved
- '<prefix-for-all-paths>': A prefix for the other variables, so the complete path does not have to be retyped each time [is optional]

### M2MT-RESREPO-003
All LINQ queries can be run separately, but to translate it all together. You should run the `LINQPadPreCompiler.linq` and after running that query you should run the translator solution.
