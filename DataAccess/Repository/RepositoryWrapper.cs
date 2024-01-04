using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Dto;

namespace DataAccess.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper 
    {
        private DapperContext _repoContext;
        private DtoWrapper _dto;
        private IEmployeeRepository _employee;
        private IHelperRepository _helper;

        public RepositoryWrapper(DapperContext repoContext, DtoWrapper dto)
        {
            _repoContext = repoContext;
            _dto = dto;
        }
        public IEmployeeRepository Employee 
        { 
            get 
            { 
                if (_employee == null) 
                {
                    _employee = new EmployeeRepository(_repoContext, _dto); 
                } 
                return _employee; 
            } 
        }

        public IHelperRepository Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new HelperRepository(_repoContext, _dto);
                }
                return _helper;
            }
        }        
        public void Save() 
        {
            _repoContext.SaveChanges();
        } 
    }
}
