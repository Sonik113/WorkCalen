using System.Security.Claims;
using System.Threading.Tasks;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.Responses;
using WorkCalendarik.Domain.ViewModels.LogAndReg;

namespace WorkCalendarik.Service.Interfaces;

public interface IAccountService
{
    Task<BaseResponse<string>> Register(RegisterViewModel model);
    
    Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    
    Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(ConfirmEmailViewModel model, string code, string confirmCode);
    
    Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model);
}