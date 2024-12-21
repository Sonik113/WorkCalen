using System.ComponentModel.DataAnnotations;

namespace WorkCalendarik.Domain.ViewModels.LogAndReg;

public class LoginViewModel
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}