using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System;
using System.IO;
using System.Linq;

using NPC.Utility;
using NPC.Runtime.Runtime;
using Rosen.EMS.Infrastructure.DynamicConditionQuery;
using Newtonsoft.Json.Linq;
using Rosen.EMS.Infrastructure.DynamicConditionQuery.Dto;
using NPC.Compiler;

namespace NPC.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonConvert.DefaultSettings = () =>
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            };
            if (args.Length == 0)
            {
                Console.WriteLine("NPC.Runtime <path to .npc> to compile and run");
                Console.WriteLine("NPC.Runtime beautify <path to .npc> to validate and beautify");
                Console.WriteLine("NPC.Runtime run <path to .json|.npc> to run");
                Console.WriteLine("Entering interpretation mode:");
                Console.WriteLine(".clrscr to clear screen");
                Console.WriteLine(".exit to exit");
                Console.WriteLine(".load <path to .npc> to compile and run");
                Console.WriteLine(".run <path to .json> to run");
                while (true)
                {
                    Console.Write("> ");
                    string text = Console.ReadLine();
                    if (text.Length == 0)
                    {
                        continue;
                    }
                    if (text.StartsWith("."))
                    {
                        if (RunCommand(text))
                        {
                            break;
                        }
                        continue;
                    }
                    text = text.TrimEnd() + ';';
                    try
                    {
                        (Compiler.AST.Policy policy, Error error) = Parse(text);
                        if (error == null)
                        {
                            ConditionStatementDto[] translated = Translate(policy);
                            Console.WriteLine("=====");
                            Console.WriteLine("beautify:");
                            PrettyPrint pp = new PrettyPrint();
                            Console.WriteLine(pp.Beautify(policy));

                            var result = JsonConvert.SerializeObject(translated, Formatting.Indented);
                            Console.WriteLine("=====");
                            Console.WriteLine("compiled:");
                            Console.WriteLine(result);

                            var matches = RunCondition(translated);
                            Console.WriteLine("=====");
                            Console.WriteLine("matches:");
                            foreach (var m in matches)
                            {
                                Console.WriteLine(m);
                            }
                        }
                        else
                        {
                            Console.WriteLine(error);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else if (args.Length == 1)
            {
                string sourcePath = args[0];
                string source = File.ReadAllText(sourcePath);
                {
                    Console.WriteLine("source:");
                    Console.WriteLine(source);

                    (Compiler.AST.Policy policy, Error error) = Parse(source);
                    if (error == null)
                    {
                        Console.WriteLine("=====");
                        Console.WriteLine("beautify:");
                        PrettyPrint pp = new PrettyPrint();
                        Console.WriteLine(pp.Beautify(policy));

                        ConditionStatementDto[] conditions = Translate(policy);
                        var serializedConditions = JsonConvert.SerializeObject(conditions, Formatting.Indented);
                        Console.WriteLine("=====");
                        Console.WriteLine("compiled:");
                        Console.WriteLine(serializedConditions);
                        serializedConditions = JsonConvert.SerializeObject(conditions, Formatting.Indented);
                        string compiledPath = Path.ChangeExtension(sourcePath, ".json");
                        File.WriteAllText(compiledPath, serializedConditions);
                        Console.WriteLine("saved to {0}", compiledPath);

                        var matches = RunCondition(conditions);

                        Console.WriteLine("=====");
                        Console.WriteLine("matches:");
                        foreach (var m in matches)
                        {
                            Console.WriteLine(m);
                        }
                    }
                }

                //Console.ReadLine();
            }
            else if (args.Length == 2)
            {
                var verb = args[0];
                var filePath = args[1];
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("'{0}' doesnt exist!", filePath);
                }
                else
                {
                    var errorPath = Path.ChangeExtension(filePath, ".err.json");
                    if (File.Exists(errorPath))
                    {
                        File.Delete(errorPath);
                    }
                    switch (verb)
                    {
                        case "compile":
                            {
                                var source = File.ReadAllText(filePath);
                                var compiledPath = Path.ChangeExtension(filePath, ".json");
                                var compilationResult = Parse(source);
                                (Compiler.AST.Policy policy, Error error) = Parse(source);
                                if (error == null)
                                {
                                    ConditionStatementDto[] conditions = Translate(policy);
                                    var serializedConditions = JsonConvert.SerializeObject(conditions, Formatting.Indented);
                                    File.WriteAllText(compiledPath, serializedConditions);
                                    Console.WriteLine("saved compiled to {0}", compiledPath);
                                }
                                else
                                {
                                    File.WriteAllText(errorPath, JsonConvert.SerializeObject(error));
                                }
                            }
                            break;
                        case "beautify":
                            {
                                var source = File.ReadAllText(filePath);
                                var beautifiedPath = Path.ChangeExtension(filePath, ".b.npc");
                                var compilationResult = Parse(source);
                                (Compiler.AST.Policy policy, Error error) = Parse(source);
                                if (error == null)
                                {
                                    PrettyPrint pp = new PrettyPrint();
                                    var beautified = pp.Beautify(policy);
                                    File.WriteAllText(beautifiedPath, beautified);
                                    Console.WriteLine("saved beautified source to {0}", beautifiedPath);
                                }
                                else
                                {
                                    File.WriteAllText(errorPath, JsonConvert.SerializeObject(error));
                                }
                            }
                            break;
                        case "run":
                            {
                                ConditionStatementDto[] conditions = null;
                                var source = File.ReadAllText(filePath);
                                switch (Path.GetExtension(filePath))
                                {
                                    case ".json":
                                        conditions = JsonConvert.DeserializeObject<ConditionStatementDto[]>(source);
                                        break;
                                    case ".npc":
                                        (Compiler.AST.Policy policy, Error error) = Parse(source);
                                        if (error == null)
                                        {
                                            conditions = Translate(policy);
                                        }
                                        else
                                        {
                                            File.WriteAllText(errorPath, JsonConvert.SerializeObject(error));
                                            Environment.Exit(0);
                                        }
                                        break;
                                }
                                var runResultPath = Path.ChangeExtension(filePath, ".r.json");
                                File.WriteAllText(runResultPath, RunCondition(conditions).ToString());
                                Console.WriteLine("saved run result to {0}", runResultPath);
                            }
                            break;
                        case "testlexer":
                            {
                                var source = File.ReadAllText(filePath);
                                (Compiler.AST.Policy policy, Error error) = Parse(source);
                                if (error == null)
                                {
                                    PrettyPrint pp = new PrettyPrint();
                                    var beautified = pp.Beautify(policy);
                                    Console.WriteLine(beautified);
                                }
                                else
                                {
                                    Console.WriteLine(error);
                                }
                                Console.ReadLine();
                            }
                            break;
                        default:
                            Console.WriteLine("Unknown command: {0}", verb);
                            break;
                    }
                }
            }
        }

        private static (Compiler.AST.Policy policy, Error error) Parse(string text)
        {
            var policy = Compiler.Compiler.Compile(text);
            return policy;
        }

        private static ConditionStatementDto[] Translate(Compiler.AST.Policy policy)
        {
            Translator translator = new Translator();
            var translated = translator.Translate(policy);
            return translated;
        }

        private static JArray RunCondition(ConditionStatementDto[] translated)
        {
            JToken input = JToken.FromObject(new
            {
                Condition = translated
            });

            return ConditionQueryHandler.HandleCondition(input);
        }

        private static bool RunCommand(string text)
        {
            try
            {
                NativeCommand cmd = NativeCommand.Parse(text);
                switch (cmd.Verb)
                {
                    case "clrscr":
                        Console.Clear();
                        break;
                    case "exit":
                        return true;
                    case "load":
                        {
                            if (cmd.Parameters.Length == 1)
                            {
                                string filePath = cmd.Parameters[0];
                                if (File.Exists(filePath))
                                {
                                    var source = File.ReadAllText(filePath);
                                    Console.WriteLine("source:");
                                    Console.WriteLine(source);

                                    (Compiler.AST.Policy policy, Error error) = Parse(source);
                                    if (error == null)
                                    {

                                        ConditionStatementDto[] conditions = Translate(policy);
                                        var serializedConditions = JsonConvert.SerializeObject(conditions, Formatting.Indented);
                                        Console.WriteLine("=====");
                                        Console.WriteLine("compiled:");
                                        Console.WriteLine(serializedConditions);

                                        var matches = RunCondition(conditions);

                                        Console.WriteLine("=====");
                                        Console.WriteLine("matches:");
                                        foreach (var m in matches)
                                        {
                                            Console.WriteLine(m);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(error);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("'{0}' doesnt exist!", filePath);
                                }
                            }
                            else
                            {
                                Console.WriteLine("load <path to .npc>");
                            }
                        }
                        break;
                    case "run":
                        {
                            if (cmd.Parameters.Length == 1)
                            {
                                string filePath = cmd.Parameters[0];
                                if (File.Exists(filePath))
                                {
                                    var compiled = File.ReadAllText(filePath);
                                    Console.WriteLine("compiled:");
                                    Console.WriteLine(compiled);

                                    ConditionStatementDto[] conditions = JsonConvert.DeserializeObject<ConditionStatementDto[]>(compiled);

                                    var matches = RunCondition(conditions);

                                    Console.WriteLine("=====");
                                    Console.WriteLine("matches:");
                                    foreach (var m in matches)
                                    {
                                        Console.WriteLine(m);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("'{0}' doesnt exist!", filePath);
                                }
                            }
                            else
                            {
                                Console.WriteLine("run <path to .json>");
                            }
                        }
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(cmd.Verb))
                        {
                            Console.WriteLine("No verb");
                        }
                        else
                        {
                            Console.WriteLine("Unknown verb: {0}", cmd.Verb);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
    }
}
