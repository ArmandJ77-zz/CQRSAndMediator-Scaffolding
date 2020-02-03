using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public interface IWithInheritance
    {
        IWithInheritance WithInheritance(List<string> inheritanceLis);
        IWithInheritance ImplementMediatorHandlerInheritance(string responseTypeName, string requestTypeName);
        void Generate();
    }
}
