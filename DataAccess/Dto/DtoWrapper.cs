using DataAccess.Dto.Request;
using DataAccess.Dto.Response;

namespace DataAccess.Dto
{
    public class DtoWrapper
    {
        private DailyAttendResDto _dailyAttendRes;
        private DailyAttendUpdateDto _dailyAttendUpdate;
        private ImageUpdateReqDto _imageUpdateReq;
        private ImageUpdateRepoDto _imageUpdateRepo;
        private EmpGetReqDto _empGetReq;
        private CheckCredentialResDto _checkCredRes;
        private HostCheckReqDto _hostCheckReq;
        private RetreiveBlobReqDto _retreiveBlobReq;
        private UserCredReqDto _userCredReq;
        private GeneralDataResDto _generalDataRes;
        private PortalDetailReqDto _portalDetailReq;
        public CheckCredentialResDto checkCredRes
        {
            get
            {
                if (_checkCredRes == null)
                {
                    _checkCredRes = new CheckCredentialResDto();
                }
                return _checkCredRes;
            }
        }

        public HostCheckReqDto hostCheckReq
        {
            get
            {
                if (_hostCheckReq == null)
                {
                    _hostCheckReq = new HostCheckReqDto();
                }
                return _hostCheckReq;
            }
        }

        public UserCredReqDto userCredkReq
        {
            get
            {
                if (_userCredReq == null)
                {
                    _userCredReq = new UserCredReqDto();
                }
                return _userCredReq;
            }
        }

        public DailyAttendResDto dailyAttendRes
        {
            get
            {
                if (_dailyAttendRes == null)
                {
                    _dailyAttendRes = new DailyAttendResDto();
                }
                return _dailyAttendRes;
            }
        }

        public DailyAttendUpdateDto dailyAttendUpdate
        {
            get
            {
                if (_dailyAttendUpdate == null)
                {
                    _dailyAttendUpdate = new DailyAttendUpdateDto();
                }
                return _dailyAttendUpdate;
            }
        }

        public ImageUpdateReqDto imageUpdateReq
        {
            get
            {
                if (_imageUpdateReq == null)
                {
                    _imageUpdateReq = new ImageUpdateReqDto();
                }
                return _imageUpdateReq;
            }
        }

        public ImageUpdateRepoDto imageUpdateRepo
        {
            get
            {
                if (_imageUpdateRepo == null)
                {
                    _imageUpdateRepo = new ImageUpdateRepoDto();
                }
                return _imageUpdateRepo;
            }
        }
       
        public EmpGetReqDto empGetReq
        {
            get
            {
                if (_empGetReq == null)
                {
                    _empGetReq = new EmpGetReqDto();
                }
                return _empGetReq;
            }
        }

        public RetreiveBlobReqDto RetreiveBlobReq
        {
            get
            {
                if (_retreiveBlobReq == null)
                {
                    _retreiveBlobReq = new RetreiveBlobReqDto();
                }
                return _retreiveBlobReq;
            }
        }

        public GeneralDataResDto GeneralDataRes
        {
            get
            {
                if (_generalDataRes == null)
                {
                    _generalDataRes = new GeneralDataResDto();
                }
                return _generalDataRes;
            }
        }

        public PortalDetailReqDto DetailsReq
        {
            get
            {
                if (_portalDetailReq == null)
                {
                    _portalDetailReq = new PortalDetailReqDto();
                }
                return _portalDetailReq;
            }
        }

       

    }
}
