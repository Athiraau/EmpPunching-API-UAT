namespace Business.Contracts
{
    public interface IServiceWrapper
    {
        IEmployeeService Employee { get; }
        IHelperService Helper { get; }
        IJwtUtils JwtUtils { get; }
    }
}
