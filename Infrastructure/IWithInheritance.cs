using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public interface IWithInheritance
    {
        IWithInheritance WithInheritance(List<string> inheritanceLis);
        IWithInheritance ImplementMediatorHandlerInheritance(string responseTypeName, string requestTypeName);
        IWithInheritance AddStartupConstructor();
        IWithInheritance AddGetProperty(string propType, string name, SyntaxKind accessModifier);
        IWithInheritance AddStartupConfigureServices();
        IWithInheritance AddStartupConfigure();
        IWithInheritance AddMethod(SyntaxToken[] modifiers,
            TypeSyntax returnType,
            string name,
            ParameterSyntax[] parameterArray,
            List<StatementSyntax> bodyStatementArray);

        void GenerateHandler();
        void Generate(string absolutePath, string className);
    }
}
