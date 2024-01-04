namespace DataAccess.Contracts
{
    public interface IRepositoryWrapper 
    { 
        IEmployeeRepository Employee { get; } 
        IHelperRepository Helper { get; }
        void Save(); 
    }
}
