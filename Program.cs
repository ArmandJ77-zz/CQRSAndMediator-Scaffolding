using CQRSAndMediator.Scaffolding.Builders;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Resolver;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using CQRSAndMediator.Scaffolding.Utilities;

namespace CQRSAndMediator.Scaffolding
{
    class Program
    {
        public static int Main(string[] args)
        {
            return RunScaffolding(args);
        }

        private static int RunScaffolding(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "Scaffolding CLI",
                Description = "A CQRS and Mediator scaffolding CLI",
                AllowArgumentSeparator = true,
            };

            app.HelpOption(true);
            //Top level command which can split into two directions
            app.Command("new", configCmd =>
            {
                configCmd.OnExecute(() =>
                {
                    Console.WriteLine("Scaffold a new solution or extend the domain");
                    configCmd.ShowHelp();
                    return 1;
                });

                //Split 1: sln
                configCmd.Command("sln", setCmd =>
                {
                    setCmd.Description =
                        "Scaffold a new solution including the API, Logic, DB, Unit and Integration test projects";
                    var nameArgument = setCmd.Option("-n| --name <NAME>","Name of the solution", CommandOptionType.SingleValue).IsRequired();

                    setCmd.OnExecute(() =>
                    {
                        var path = Directory.GetCurrentDirectory();
                        BuilderSolution.Build(nameArgument.Value());
                        BuildStartup.Build($"{path}/API");
                        BuildCorsExtension.Build($"{path}/API/Extensions");
                        BuildHealthCheckExtensions.Build($"{path}/API/Extensions");
                        BuildDatabaseServiceCollectionExtensions.Build($"{path}/DB/Configuration");
                        BuildLogicServiceCollectionExtensions.Build($"{path}/Logic/Configuration");

                        Console.WriteLine(ExecuteCommandUtility.Run($"echo [92mDONE[0m"));
                    });
                });

                //Split 2: domain
                configCmd.Command("domain", setCmd =>
                {
                    setCmd.Description = "Extent the domain with new handlers";

                    var operationType = setCmd.Option(
                            "-ot|--operationType <TYPE>",
                            "Can either be [command] or [query]",
                            CommandOptionType.SingleValue)
                        .IsRequired(false,
                            "Must specify an operation type: Can either be [command] or [query] i.e -ot|--operationType <TYPE>");

                    var concern = setCmd.Option(
                            "-c|--concern <NAME>",
                            "Name of the concern",
                            CommandOptionType.SingleValue)
                        .IsRequired(false, "Name of the concern: -c|--concern <NAME>");

                    var operation = setCmd.Option(
                            "-o|--operation <NAME>",
                            "Name of the operation",
                            CommandOptionType.SingleValue)
                        .IsRequired(false, "Name of the operation: -o|--operation <NAME>");

                    var groupBy = setCmd.Option(
                        "-g|--groupBy <TYPE>",
                        "Group domain objects by [C] for concerns or [O] for operations, defaults to concerns",
                        CommandOptionType.SingleValue);

                    setCmd.OnExecute(() =>
                    {
                        var groupByType = GroupByType.Concern;
                        if (groupBy.HasValue())
                        {
                            groupByType = (groupBy.Value()?.ToLower()) switch
                            {
                                "c" => GroupByType.Concern,
                                "o" => GroupByType.Operation,
                                _ => GroupByType.Concern
                            };
                        }

                        var operationTypeBuilderResult = OperationTypeResolver.Resolve(operationType.Value());
                        if (operationTypeBuilderResult == OperationType.UNSUPPORTED)
                        {
                            LogUtility.Error("Invalid operation type parameter: must specify [c] for command or [q] for query");
                            return 0;
                        }

                        BuildResponse.Build(concern.Value(), operation.Value(), groupByType);

                        switch (operationTypeBuilderResult)
                        {
                            case OperationType.COMMAND:
                                BuildCommand.Build(concern.Value(), operation.Value(), groupByType);
                                break;
                            case OperationType.QUERY:
                                BuildQuery.Build(concern.Value(), operation.Value(), groupByType);
                                break;
                            case OperationType.UNSUPPORTED:
                                LogUtility.Error(
                                    "Invalid operation type parameter: must specify [c] for command or [q] for query");
                                break;
                            default:
                                LogUtility.Error(
                                    "Invalid operation type parameter: must specify [c] for command or [q] for query");
                                break;
                        };

                        BuildHandler.Build(concern.Value(), operation.Value(), operationTypeBuilderResult, groupByType);

                        return 0;
                    });
                });
            });

            //Split 3 : Message bus
            //TODO: Add scaffolding to create messagebus consumer or producer

            //Split 3 : Redis cache
            //TODO: Add scaffolding to create either a cached api route or cached resource

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }
    }
}
