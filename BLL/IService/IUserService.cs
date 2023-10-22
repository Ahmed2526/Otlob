using DAL.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface IUserService
    {
        Task<CommonApiResponse<UserDto>> Login(LoginDto loginCredentials);
        Task<CommonApiResponse<UserDto>> Register(RegisterDto registerCredentials);

    }
}
