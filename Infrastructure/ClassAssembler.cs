using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using CQRSAndMediator.Scaffolding.Utilities;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public sealed class ClassAssembler : IOnConfiguration, IWithNamespace, IWithInheritance
    {
        private DomainSettingsModel _settings;
        private CompilationUnitSyntax _syntaxFactory;
        private NamespaceDeclarationSyntax _namespace;
        private ClassDeclarationSyntax _class;
        public ClassAssembler(string concern, string operation, PatternDirectoryType patternType, GroupByType groupBy)
        {
            _settings = new DomainSettingsModel(concern, operation, patternType, groupBy);
            _syntaxFactory = SyntaxFactory.CompilationUnit();
        }
        public static IOnConfiguration Configure(string concern, string operation, PatternDirectoryType patternType, GroupByType groupBy)
            => new ClassAssembler(concern, operation, patternType, groupBy);

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
        public IWithNamespace CreateNamespace()
        {
            var name = _settings.GroupingStrategy switch
            {
                GroupByType.Concern => $"{_settings.DomainName}.{_settings.Concern}.{_settings.PatternType.ToString()}",
                GroupByType.Operation => $"{_settings.DomainName}.{_settings.PatternType.ToString()}.{_settings.Concern}"
            };

            _namespace = SyntaxFactory.NamespaceDeclaration(
                SyntaxFactory.ParseName(name))
                .NormalizeWhitespace();

            return this;
        }
        public IWithInheritance CreateClass()
        {
            LogUtility.Info($"class Name: ${_settings.ClassName}");
            _class = SyntaxFactory.ClassDeclaration(_settings.ClassName);
            _class = _class.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

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
        public void Generate()
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
            LogUtility.Info($"patternAbsolutePath: ${patternAbsolutePath}");

            var concernAbsolutePath = _settings.GroupingStrategy switch
            {
                GroupByType.Concern => FileSystemUtility.CreateDirectory(new[] { _settings.DomainAbsolutePath, _settings.Concern, _settings.PatternType.ToString() }),
                GroupByType.Operation => FileSystemUtility.CreateDirectory(new[] { patternAbsolutePath, _settings.Concern })
            };
            LogUtility.Info($"concernAbsolutePath: ${concernAbsolutePath}");

            var absoluteFilePath = FileSystemUtility.CreateFile(new[] { concernAbsolutePath, _settings.ClassName }, data);
            LogUtility.Info($"absoluteFilePath: ${absoluteFilePath}");

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
