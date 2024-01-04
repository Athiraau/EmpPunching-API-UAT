using DataAccess.Contracts;
using Business.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using DataAccess.Dto;

namespace Business.Services
{
    public class ServiceWrapper: IServiceWrapper
    {
        private IEmployeeService _employee;
        private IHelperService _helper;
        private IJwtUtils _jwtUtils;

        private readonly IRepositoryWrapper _repository;
        private readonly ServiceHelper _service;
        private readonly IConfiguration _config;
        private readonly ILoggerService _logger;
        private readonly IWebHostEnvironment _env;
        private readonly DtoWrapper _dto;
        public ServiceWrapper(IRepositoryWrapper repository, ServiceHelper service, IConfiguration config,
            ILoggerService logger, IWebHostEnvironment env, DtoWrapper dto)
        {
            _repository = repository;
            _service = service;
            _config = config;
            _logger = logger;
            _env = env;
            _dto = dto;
        }

        public IEmployeeService Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeService(_repository, _service, _config, _dto);
                }
                return _employee;
            }
        }

        public IHelperService Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new HelperService(_repository, _service, _env, _config);
                }
                return _helper;
            }
        }

        public IJwtUtils JwtUtils
        {
            get
            {
                if (_jwtUtils == null)
                {
                    _jwtUtils = new JwtUtils(_config, _logger);
                }
                return _jwtUtils;
            }
        }
    }
}
