using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Database.Responses;
using WorkCalendarik.Domain.Database.Storage;
using WorkCalendarik.Domain.Helpers;
using WorkCalendarik.Domain.Interfaces;
using WorkCalendarik.Domain.Validation.Validators;
using WorkCalendarik.Domain.ViewModels.LogAndReg;
using WorkCalendarik.Service.Interfaces;

namespace WorkCalendarik.Service.Realizations;

public class AccountService : IAccountService
{
    private string _accountName {  get; set; }

    private readonly UserStorage _userStorage;

    private readonly LoginValidator _validationRulesLogin;
    private readonly RegisterValidator _validationRulesRegister;
    private readonly UserValidator _validationRulesUser;
    private readonly IMapper _mapper;

    public AccountService(IBaseStorage<UserDb> userStorage, IMapper mapper)
    {
        _userStorage = userStorage as UserStorage;
        _mapper = mapper;
        _validationRulesLogin = new LoginValidator();
        _validationRulesRegister = new RegisterValidator();
        _validationRulesUser = new UserValidator();
    }

    public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
    {
        try
        {
            await _validationRulesLogin.ValidateAndThrowAsync(model);

            var userDb = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);

            if (userDb == null || userDb.Password != HashPasswordHelper.HashPassword(model.Password))
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = "Неверный email или пароль",
                    StatusCode = StatusCode.BadRequest
                };
            }

            var claimsIdentity = AuthenticateUserHelper.Authenticate(new User
            {
                Login = userDb.Login,
                Email = userDb.Email,
                ImagePath = userDb.ImagePath,
                Role = userDb.Role,
                CreatedAt = userDb.CreatedAt
            });

            return new BaseResponse<ClaimsIdentity>
            {
                Data = claimsIdentity,
                StatusCode = StatusCode.OK
            };
        }
        catch (ValidationException e)
        {
            return new BaseResponse<ClaimsIdentity>
            {
                Description = string.Join("; ", e.Errors.Select(x => x.ErrorMessage)),
                StatusCode = StatusCode.BadRequest
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<ClaimsIdentity>
            {
                Description = e.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<string>> Register(RegisterViewModel model)
    {
        try
        {
            await _validationRulesRegister.ValidateAndThrowAsync(model);

            if (await _userStorage.GetAll().AnyAsync(x => x.Email == model.Email))
            {
                return new BaseResponse<string>
                {
                    Description = "Пользователь с такой почтой уже существует",
                    StatusCode = StatusCode.BadRequest
                };
            }

            string confirmationCode = new Random().Next(100000, 999999).ToString();
            await SendEmail(model.Email, confirmationCode);

            return new BaseResponse<string>
            {
                Data = confirmationCode,
                Description = "Код подтверждения отправлен",
                StatusCode = StatusCode.OK
            };
        }
        catch (ValidationException e)
        {
            return new BaseResponse<string>
            {
                Description = string.Join("; ", e.Errors.Select(x => x.ErrorMessage)),
                StatusCode = StatusCode.BadRequest
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<string>
            {
                Description = e.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(ConfirmEmailViewModel model, string code, string confirmCode)
    {
        try
        {
            if (code != confirmCode)
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = "Неверный код подтверждения",
                    StatusCode = StatusCode.BadRequest
                };
            }

            var userDb = new UserDb
            {
                Login = model.Login,
                Email = model.Email,
                Password = HashPasswordHelper.HashPassword(model.Password),
                ImagePath = @"E:\Работы для технаря\практика\Calendar\WorkCalendarik\wwwroot\images\avatars\default.png",
                Role = 1
            };

            await _userStorage.Add(userDb);

            var claimsIdentity = AuthenticateUserHelper.Authenticate(new User
            {
                Login = userDb.Login,
                Email = userDb.Email,
                ImagePath = userDb.ImagePath,
                Role = userDb.Role,
                CreatedAt = userDb.CreatedAt
            });

            return new BaseResponse<ClaimsIdentity>
            {
                Data = claimsIdentity,
                Description = "Регистрация завершена",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<ClaimsIdentity>
            {
                Description = e.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    private async Task SendEmail(string email, string confirmationCode)
    {
        var emailMessage = new MimeMessage
        {
            From = { new MailboxAddress("WorkCalendarik", "ilya.cheban.300706@gmail.com") },
            To = { new MailboxAddress("", email) },
            Subject = "Код подтверждения",
            Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<html>" + "<head>" + "<style>" +
                       "body { font-family: Arial, sans-serif; background-color: #f2f2f2; }" +
                       ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); }" +
                       ".header { text-align: center; margin-bottom: 20px; }" +
                       ".message { font-size: 16px; line-height: 1.6; }" +
                       ".container-code { background-color: #f0f0f0; padding: 5px; border-radius: 5px; font-weight: bold; }" +
                       ".code {text-align: center; }" +
                       "</style>" +
                       "</head>" +
                       "<body>" +
                       "<div class='container'>" +
                       "<div class='header'><h1>Добро пожаловать на сайт WorkCalendarik!</h1></div>" +
                       "<div class='message'>" +
                       "<p>Пожалуйста, введите данный код на сайте, чтобы подтвердить ваш email и завершить регистрацию:</p>" +
                       "<div class='container-code'><p class='code'>" + confirmationCode + "</p></div>" +
                       "</div>" + "</div>" + "</body>" + "</html>"
            }
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 465, true);

        string password =
            await File.ReadAllTextAsync(
                "E:\\Работы для технаря\\практика\\Calendar\\materials\\passwordPractice.txt");
        await client.AuthenticateAsync("ilya.cheban.300706@gmail.com", password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model)
    {
        try
        {
            var userDb = new UserDb();
            if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) == null)
            {
                model.Password = "google";

                await _userStorage.Add(userDb);

                var resultRegister = AuthenticateUserHelper.Authenticate(model);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultRegister,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.OK
                };
            }

            var resultLogin = AuthenticateUserHelper.Authenticate(model);
            return new BaseResponse<ClaimsIdentity>()
            {
                Data = resultLogin,
                Description = "Объект уже был создан",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<ClaimsIdentity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
    public async Task<BaseResponse<User>> GetAccountByEmail(string email)
    {
        try
        {
            var userDb = await _userStorage.GetByEmailAsync(email);
            if (userDb == null)
            {
                return new BaseResponse<User>
                {
                    Description = "Пользователь не найден",
                    StatusCode = StatusCode.NotFound
                };
            }

            var result = _mapper.Map<User>(userDb);

            return new BaseResponse<User>
            {
                Data = result,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<User>
            {
                Description = e.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<User>> UpdateAccount(User user)
    {
        try
        {
            var userDb = await _userStorage.Get(user.Id);

            if (userDb == null)
            {
                return new BaseResponse<User>
                {
                    Description = "Пользователь не найден",
                    StatusCode = StatusCode.NotFound
                };
            }

            userDb.Login = user.Login;
            if (!string.IsNullOrEmpty(user.Password))
            {
                userDb.Password = HashPasswordHelper.HashPassword(user.Password);
            }
            userDb.Role = user.Role;
            userDb.ImagePath = user.ImagePath;
            userDb.Email = user.Email;
            userDb.CreatedAt = user.CreatedAt;

            await _userStorage.Update(userDb);

            return new BaseResponse<User>
            {
                Data = _mapper.Map<User>(userDb),
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<User>
            {
                Description = "Внутренняя ошибка сервера",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
} 