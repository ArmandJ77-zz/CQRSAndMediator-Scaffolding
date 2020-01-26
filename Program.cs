using CQRSAndMediator.Scaffolding.Builders;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSTemplates.Builders;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace CQRSAndMediator.Scaffolding
{
    class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Started");

            var app = new CommandLineApplication();

            app.HelpOption();

            var operationType = app.Option("-ot|--operationType <TYPE>", "Can either be [command] or [query]",
                CommandOptionType.SingleValue);

            var concern = app.Option("-c|--concern <NAME>", "Name of the concern",
                CommandOptionType.SingleValue);

            var operation = app.Option("-o|--operation <NAME>", "Name of the operation",
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
                    Log.Error("Invalid concern parameter: concern must be specified");
                    return 0;
                }

                var operationTypeBuilderResult = BuildOperationType.Build(operationType.Value());

                if (operationTypeBuilderResult == OperationType.UNSUPPORTED)
                {
                    Log.Error("Invalid operation type parameter: must specify [c] for command or [q] for query");
                    return 0;
                }

                BuildResponse.Build(concern.Value(), operation.Value());

                if (operationTypeBuilderResult == OperationType.COMMAND)
                    BuildCommand.Build(concern.Value(), operation.Value());

                if (operationTypeBuilderResult == OperationType.QUERY)
                    BuildQuery.Build(concern.Value(), operation.Value());

                BuildHandler.Build(concern.Value(), operation.Value(), operationTypeBuilderResult);

                return 0;
            });

            return app.Execute(args);
        }
    }
}
