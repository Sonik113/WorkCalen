namespace WorkCalendarik.Domain.Validation;

public static class ValidationMessages
{
    // Category validation messages
    public const string CategoryNameRequired = "Название категории не может быть пустым.";
    public const string CategoryNameLength = "Название категории должно быть от 1 до 100 символов.";
    public const string CategoryImagePathLength = "Путь к изображению не должен превышать 255 символов.";
    public const string CategoryCountBronCalendarsNonNegative = "Количество постов не может быть отрицательным.";

    // User Validation Messages
    public static string UserLoginRequired = "Логин обязателен для заполнения.";
    public static string UserLoginLength = "Длина логина не должна превышать 50 символов.";
    public static string LoginInvalid = "Логин содержит недопустимые символы.";
    public static string UserPasswordRequired = "Пароль обязателен для заполнения.";
    public static string UserPasswordLength = "Пароль должен содержать не менее 6 символов.";
    public static string PasswordInvalid = "Пароль не соответствует требованиям безопасности.";
    public static string UserEmailRequired = "Электронная почта обязательна для заполнения.";
    public static string UserEmailInvalid = "Неверный формат электронной почты.";
    public static string UserRoleRange = "Роль должна быть в пределах от 1 до 3.";
    public static string UserImagePathMaxLength = "Путь к изображению не должен превышать 200 символов.";
    public static string UserCreatedAtValid = "Дата создания не может быть в будущем.";
    public static string PasswordMismatch = "Пароли должны совпадать.";
    
    public static string RegCodeConfirmRequired = "Код подтверждения обязателен для заполнения.";
    public static string RegCodeConfirmLength = "Длина кода должна быть 6 символов.";
    public static string RegCodeConfirmInvalid = "Код подтверждения должен содержать только 6 цифр.";
    
    // BronCalendar validation messages
    public const string BronCalendarCarIdRequired = "ID календаря не может быть пустым.";
    public const string BronCalendarCategoryIdRequired = "ID категории не может быть пустым.";
    public const string BronCalendarDescriptionRequired = "Описание поста не может быть пустым.";
    public const string BronCalendarDescriptionLength = "Описание должно быть от 10 до 1000 символов.";
    public const string BronCalendarPricePositive = "Цена должна быть больше нуля.";
    public const string BronCalendarAvailabilityStatusRequired = "Статус доступности не может быть пустым.";
    public const string BronCalendarCreatedAtRequired = "Дата создания календаря не может быть пустой.";

    // Rate validation messages
    public const string RateUserIdRequired = "ID пользователя не может быть пустым.";
    public const string RateCommentLength = "Комментарий не должен превышать 500 символов.";
    public const string RatePointsRange = "Оценка должна быть в пределах от 1 до 5.";
    public const string RateDateRequired = "Дата отзыва не может быть пустой.";
}

