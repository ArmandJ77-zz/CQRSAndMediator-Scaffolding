using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public interface IWithNamespace
    {

        IWithNamespace CreateNamespace();

        IWithInheritance CreateClass();


    }
}
