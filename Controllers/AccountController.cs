using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Helpers;
using WorkCalendarik.Domain.Interfaces;
using WorkCalendarik.Domain.ViewModels.Account;
using WorkCalendarik.Service.Interfaces;
using WorkCalendarik.Service.Realizations;

namespace WorkCalendarik.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IBaseStorage<UserDb> _accountStorage;
    private IMapper _mapper;

    private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });

    public AccountController(IAccountService accountService, IBaseStorage<UserDb> accountStorage, IMapper mapper)
    {
        _accountService = accountService;
        _accountStorage = accountStorage;
        _mapper = mapperConfiguration.CreateMapper();
    }

    [HttpGet("/Account/AccountPage/{email}")]
    public async Task<IActionResult> AccountPage(string email)
    {
        var resultAccount = await _accountService.GetAccountByEmail(email);
        if (resultAccount.StatusCode != Domain.Database.Responses.StatusCode.OK || resultAccount.Data == null)
        {
            return NotFound();
        }

        AccountPageViewModel accountPage = _mapper.Map<AccountPageViewModel>(resultAccount.Data);
        return View(accountPage);
    }

    [HttpPost("/Account/UpdateAccount")]
    public async Task<IActionResult> UpdateAccount(AccountPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("AccountPage", model);
        }

        var userDb = await _accountService.GetAccountByEmail(model.Email);
        if (userDb.StatusCode != Domain.Database.Responses.StatusCode.OK || userDb.Data == null)
        {
            ModelState.AddModelError("", "Пользователь не найден.");
            return View("AccountPage", model);
        }

        var userToUpdate = userDb.Data;

        if (!string.IsNullOrEmpty(model.Password))
        {
            if (model.Password != model.PasswordConfirm)
            {
                ModelState.AddModelError("", "Пароли не совпадают.");
                return View("AccountPage", model);
            }
            userToUpdate.Password = HashPasswordHelper.HashPassword(model.Password);
        }

        userToUpdate.Login = model.Login;
        userToUpdate.Email = model.Email;
        userToUpdate.Role = model.Role;
        userToUpdate.ImagePath = string.IsNullOrEmpty(model.ImagePath) ? userToUpdate.ImagePath : model.ImagePath;

        var updateResult = await _accountService.UpdateAccount(userToUpdate);

        if (updateResult.StatusCode != Domain.Database.Responses.StatusCode.OK)
        {
            ModelState.AddModelError("", updateResult.Description ?? "Ошибка при сохранении данных.");
            return View("AccountPage", model);
        }

        return RedirectToAction("AccountPage", new { email = model.Email });
    }

}