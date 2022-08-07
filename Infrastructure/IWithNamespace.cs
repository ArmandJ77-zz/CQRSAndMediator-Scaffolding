using Microsoft.CodeAnalysis;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public interface IWithNamespace
    {
        IWithNamespace CreateNamespace(string name = null);
        IWithInheritance CreateClass(SyntaxToken[] modifiers, string name = null);
    }
}
