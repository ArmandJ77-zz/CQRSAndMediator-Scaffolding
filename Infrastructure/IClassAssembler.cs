namespace CQRSAndMediator.Scaffolding.Infrastructure
{
    public interface IClassAssembler
    {
        IClassAssembler WithNamespace();
       
        IClassAssembler WithClassName();
        void Create();
    }
}
