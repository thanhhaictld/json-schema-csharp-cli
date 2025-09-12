using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JsonSchemaToCSharpCli
{
    class Program
    {
        private const string Version = "1.0.0";

        public static int Main(string[] args)
        {
            return MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task<int> MainAsync(string[] args)
        {
            // Create command-line options
            var schemaPathArgument = new Argument<string>(
                name: "schema-path",
                description: "Input JSON schema file path");

            var outputPathArgument = new Argument<string>(
                name: "output-path",
                description: "Output C# file path");

            var namespaceOption = new Option<string>(
                name: "--namespace",
                description: "Namespace for generated C# classes",
                getDefaultValue: () => "Generated");
            namespaceOption.AddAlias("-n");

            var versionOption = new Option<bool>(
                name: "--version",
                description: "Display version information");
            versionOption.AddAlias("-v");

            // Create root command
            var rootCommand = new RootCommand("JSON Schema to C# class generator");
            rootCommand.AddArgument(schemaPathArgument);
            rootCommand.AddArgument(outputPathArgument);
            rootCommand.AddOption(namespaceOption);

            rootCommand.SetHandler(async (string schemaPath, string outputPath, string namespaceName, bool showVersion) =>
            {
                if (showVersion)
                {
                    Console.WriteLine($"JsonSchemaToCSharpCli version {Version}");
                    return;
                }

                try
                {
                    // Load the JSON schema file
                    var schema = await JsonSchema.FromFileAsync(schemaPath);

                    // Generate C# code from the schema
                    var settings = new CSharpGeneratorSettings
                    {
                        Namespace = namespaceName,
                        TypeAccessModifier = "public",
                        GenerateDataAnnotations = true,
                        JsonLibrary = CSharpJsonLibrary.SystemTextJson,
                    };

                    var generator = new CSharpGenerator(schema, settings);
                    var code = generator.GenerateFile();

                    // if directory does not exist, create it
                    var outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Write the generated code to the output file
                    File.WriteAllText(outputPath, code);
                    Console.WriteLine($"C# classes generated successfully at {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating C# code: {ex.Message}");
                    Environment.Exit(1);
                }
            }, schemaPathArgument, outputPathArgument, namespaceOption, versionOption);

            // Parse the command line
            return await rootCommand.InvokeAsync(args);
        }
    }
}
