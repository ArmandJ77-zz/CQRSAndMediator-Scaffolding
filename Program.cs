using CQRSAndMediator.Scaffolding.Builders;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using McMaster.Extensions.CommandLineUtils;
using System;
using CQRSAndMediator.Scaffolding.Resolver;

namespace CQRSAndMediator.Scaffolding
{
    class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Started");

            var app = new CommandLineApplication();
            var groupByType = GroupByType.Concern;

            app.HelpOption();

            var operationType = app.Option(
                "-ot|--operationType <TYPE>",
                "Can either be [command] or [query]",
                CommandOptionType.SingleValue);

            var concern = app.Option(
                "-c|--concern <NAME>",
                "Name of the concern",
                CommandOptionType.SingleValue);

            var operation = app.Option(
                "-o|--operation <NAME>",
                "Name of the operation",
                CommandOptionType.SingleValue);

            var groupBy = app.Option(
                "-g|--groupBy <TYPE>",
                "Group domain objects by [C] for concerns or [O] for operations, defaults to concerns",
                CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                if (!operationType.HasValue())
                {
                    Log.Error("Invalid operation type parameter: must specify [c] for command or [q] for query");
                    return 0;
                }

                if (!concern.HasValue())
                {
                    Log.Error("Invalid concern parameter: concern must be specified");
                    return 0;
                }

                if (!operation.HasValue())
                {
                    Log.Error("Invalid operation parameter: operation must be specified");
                    return 0;
                }

                if (groupBy.HasValue())
                {
                    groupByType = (groupBy.Value().ToLower()) switch
                    {
                        "c" => GroupByType.Concern,
                        "o" => GroupByType.Operation,
                        _ => GroupByType.Concern
                    };
                }

                var operationTypeBuilderResult = OperationTypeResolver.Resolve(operationType.Value());

                if (operationTypeBuilderResult == OperationType.UNSUPPORTED)
                {
                    Log.Error("Invalid operation type parameter: must specify [c] for command or [q] for query");
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
                        Log.Error("Invalid operation type parameter: must specify [c] for command or [q] for query");
                        break;
                    default:
                        Log.Error("Invalid operation type parameter: must specify [c] for command or [q] for query");
                        break;
                };

                BuildHandler.Build(concern.Value(), operation.Value(), operationTypeBuilderResult, groupByType);

                return 0;
            });

            return app.Execute(args);
        }
    }
}
