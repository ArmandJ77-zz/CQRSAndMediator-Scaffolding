namespace CQRSAndMediator.Scaffolding.Models
{
    public class NamespaceModel
    {
        public string Name { get; private set; }
        public bool PrependWithDomainName { get; }

        public NamespaceModel(string name, bool prependWithDomainName = false)
        {
            Name = name;
            PrependWithDomainName = prependWithDomainName;
        }
        public void AddDomainNamespace(string domain) => Name = $"{domain}.{Name}";
    }
}
