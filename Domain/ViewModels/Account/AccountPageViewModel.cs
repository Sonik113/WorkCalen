namespace WorkCalendarik.Domain.ViewModels.Account;

public class AccountPageViewModel
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirm { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}
