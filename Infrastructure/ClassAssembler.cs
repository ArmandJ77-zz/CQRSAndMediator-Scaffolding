using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Models;
using CQRSAndMediator.Scaffolding.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public sealed class ClassAssembler : IOnConfiguration, IWithNamespace, IWithInheritance
    {
        private readonly DomainSettingsModel _settings;
        private CompilationUnitSyntax _syntaxFactory;
        private NamespaceDeclarationSyntax _namespace;
        private ClassDeclarationSyntax _class;

        public ClassAssembler()
        {
            _syntaxFactory = SyntaxFactory.CompilationUnit();
        }

        public ClassAssembler(string concern, string operation, PatternDirectoryType patternType, GroupByType groupBy)
        {
            _settings = new DomainSettingsModel(concern, operation, patternType, groupBy);
            _syntaxFactory = SyntaxFactory.CompilationUnit();
        }

        public static IOnConfiguration ConfigureHandler(string concern, string operation, PatternDirectoryType patternType, GroupByType groupBy)
            => new ClassAssembler(concern, operation, patternType, groupBy);

        public static IOnConfiguration Configure()
            => new ClassAssembler();

        public IWithNamespace ImportNamespaces(List<NamespaceModel> namespaceModels = null)
        {
            namespaceModels?.ForEach(model =>
            {
                if (model.PrependWithDomainName)
                    model.Name = $"{_settings.DomainName}.{model.Name}";

                _syntaxFactory = _syntaxFactory.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(model.Name)));
            });

            return this;
        }

        public IWithNamespace CreateNamespace(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                name = _settings.GroupingStrategy switch
                {
                    GroupByType.Concern => $"{_settings.DomainName}.{_settings.Concern}.{_settings.PatternType.ToString()}",
                    GroupByType.Operation => $"{_settings.DomainName}.{_settings.PatternType.ToString()}.{_settings.Concern}"
                };

            _namespace = SyntaxFactory.NamespaceDeclaration(
                SyntaxFactory.ParseName(name))
                .NormalizeWhitespace();

            return this;
        }
        public IWithInheritance CreateClass(SyntaxToken[] modifiers, string name = null)
        {
            if (string.IsNullOrEmpty(name))
                name = _settings.ClassName;

            LogUtility.Info($"Adding class: ${name}");
            _class = SyntaxFactory.ClassDeclaration(name);
            _class = _class.AddModifiers(modifiers);

            return this;
        }

        public IWithInheritance AddStartupConstructor()
        {
            var syntax = SyntaxFactory.ParseStatement("Configuration = configuration;");

            var parameterList = new[]
            {
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("configuration"))
                    .WithType(SyntaxFactory.ParseTypeName("IConfiguration"))
            };

            var constructorDeclaration = SyntaxFactory.ConstructorDeclaration("Startup")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(parameterList)
                .WithBody(SyntaxFactory.Block(syntax));

            _class = _class.AddMembers(constructorDeclaration);
            return this;
        }

        public IWithInheritance AddGetProperty(string propType, string name, SyntaxKind accessModifier)
        {
            var propertyDeclaration = SyntaxFactory
                .PropertyDeclaration(SyntaxFactory.ParseTypeName(propType), name)
                .AddModifiers(SyntaxFactory.Token(accessModifier))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            _class = _class.AddMembers(propertyDeclaration);

            return this;

        }

        public IWithInheritance AddStartupConfigureServices()
        {
            var statements = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement("services").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddCorsRules()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddControllers()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddNewtonsoftJson()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(";").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement("").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),

                SyntaxFactory.ParseStatement("services").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddMvc()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(";").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement("").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),

                SyntaxFactory.ParseStatement("services").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddMicroserviceHealthChecks()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddDatabase(Configuration)").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".AddLogic(Configuration)").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(";").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
            };

            var parameterList = new[]
            {
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("services"))
                    .WithType(SyntaxFactory.ParseTypeName("IServiceCollection")),
            };

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName($"void"), "ConfigureServices")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(parameterList)
                .WithBody(SyntaxFactory.Block(statements));

            _class = _class.AddMembers(methodDeclaration);

            return this;
        }

        public IWithInheritance AddStartupConfigure()
        {
            var statements = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement(@"if (env.IsEnvironment(""Local"") || env.IsDevelopment())"),
                SyntaxFactory.ParseStatement("app.UseDeveloperExceptionPage();").WithLeadingTrivia(SyntaxFactory.Tab),
                SyntaxFactory.ParseStatement("").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),

                SyntaxFactory.ParseStatement("app").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".UseRouting()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".UseAuthorization()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".UseEndpoints(endpoints => endpoints.MapControllers())").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(".UseMicroserviceHealthChecks()").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(";").WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
            };

            var parameterList = new[]
            {
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("app"))
                    .WithType(SyntaxFactory.ParseTypeName("IApplicationBuilder")),
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("env"))
                    .WithType(SyntaxFactory.ParseTypeName("IWebHostEnvironment")),
            };

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName($"void"), "Configure")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(parameterList)
                .WithBody(SyntaxFactory.Block(statements));

            _class = _class.AddMembers(methodDeclaration);

            return this;
        }

        public IWithInheritance AddMethod(SyntaxToken[] modifiers, TypeSyntax returnType, string name, ParameterSyntax[] parameterArray, List<StatementSyntax> bodyStatementArray)
        {
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, name)
                .AddModifiers(modifiers)
                .AddParameterListParameters(parameterArray)
                .WithBody(SyntaxFactory.Block(bodyStatementArray));

            _class = _class.AddMembers(methodDeclaration);
            return this;
        }

        public IWithInheritance WithInheritance(List<string> inheritanceList)
        {
            if (inheritanceList == null)
                return this;

            foreach (var obj in inheritanceList)
            {
                _class = _class.AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(obj)));
            }

            return this;
        }

        public IWithInheritance ImplementMediatorHandlerInheritance(string responseTypeName, string requestTypeName)
        {
            var handleSyntax = SyntaxFactory.ParseStatement("throw new System.NotImplementedException();");
            var handleParameterList = new[]
            {
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("request")).WithType(SyntaxFactory.ParseTypeName(requestTypeName)),
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("cancellationToken")).WithType(SyntaxFactory.ParseTypeName("CancellationToken")),
            };

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName($"Task<{responseTypeName}>"), "Handle")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(handleParameterList)
                .WithBody(SyntaxFactory.Block(handleSyntax));

            _class = _class.AddMembers(methodDeclaration);

            return this;
        }


        public void GenerateHandler()
        {
            _namespace = _namespace.AddMembers(_class);

            _syntaxFactory = _syntaxFactory.AddMembers(_namespace);

            var data = _syntaxFactory
                .NormalizeWhitespace()
                .ToFullString();

            var patternAbsolutePath = _settings.GroupingStrategy switch
            {
                GroupByType.Concern => FileSystemUtility.CreateDirectory(new[] { _settings.DomainAbsolutePath, _settings.Concern, _settings.PatternType.ToString() }),
                GroupByType.Operation => FileSystemUtility.CreateDirectory(new[] { _settings.DomainAbsolutePath, _settings.PatternType.ToString() })
            };
            //LogUtility.Info($"patternAbsolutePath: ${patternAbsolutePath}");

            var concernAbsolutePath = _settings.GroupingStrategy switch
            {
                GroupByType.Concern => FileSystemUtility.CreateDirectory(new[] { _settings.DomainAbsolutePath, _settings.Concern, _settings.PatternType.ToString() }),
                GroupByType.Operation => FileSystemUtility.CreateDirectory(new[] { patternAbsolutePath, _settings.Concern })
            };
            //LogUtility.Info($"concernAbsolutePath: ${concernAbsolutePath}");

            var absoluteFilePath = FileSystemUtility.CreateFile(new[] { concernAbsolutePath, _settings.ClassName }, data);
            //LogUtility.Info($"absoluteFilePath: ${absoluteFilePath}");

            CleanUp();
        }

        public void Generate(string absolutePath, string className)
        {
            _namespace = _namespace.AddMembers(_class);

            _syntaxFactory = _syntaxFactory.AddMembers(_namespace);

            var data = _syntaxFactory
                .NormalizeWhitespace()
                .ToFullString();

            var absoluteFilePath = FileSystemUtility.CreateFile(new[] { absolutePath, className }, data);
            //LogUtility.Info($"absoluteFilePath: ${absoluteFilePath}");

            CleanUp();
        }

        private void CleanUp()
        {
            _class = null;
            _namespace = null;
            _syntaxFactory = null;
        }
    }
}
