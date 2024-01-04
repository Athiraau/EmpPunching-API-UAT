using Business.Helpers;
using DataAccess.Contracts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class ServiceHelper
    {
        private Helper _pHelper;
        private ValidationHelper _vHelper;

        private readonly IRepositoryWrapper _repository;
        private readonly ErrorResponse _error;

        public ServiceHelper(IRepositoryWrapper repository, ErrorResponse error)
        {
            _repository = repository;
            _error = error;
        }
       
        public Helper PHelper
        {
            get
            {
                if (_pHelper == null)
                {
                    _pHelper = new Helper(_repository);
                }
                return _pHelper;
            }
        }

        public ValidationHelper VHelper
        {
            get
            {
                if (_vHelper == null)
                {
                    _vHelper = new ValidationHelper(_error);
                }
                return _vHelper;
            }
        }
    }
}
