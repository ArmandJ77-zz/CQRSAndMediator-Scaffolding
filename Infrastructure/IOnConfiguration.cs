using System.Collections.Generic;
using CQRSAndMediator.Scaffolding.Models;

namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public interface IOnConfiguration
    {
        IWithNamespace ImportNamespaces(List<NamespaceModel> namespaceModels = null);

    }
}
