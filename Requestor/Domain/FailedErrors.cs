namespace Requestor.Domain;

public enum Languages
{
    UA
}

public class FailedErrors
{
    private static readonly Dictionary<string, Dictionary<Languages,string>> ErrorMessages;

    static FailedErrors()
    {
        ErrorMessages = new Dictionary<string, Dictionary<Languages,string>>();
        
        ErrorMessages.Add("Operation failed", new Dictionary<Languages, string>
        {
            { Languages.UA, "Операція не вдалася" }, 
        });
        
        ErrorMessages.Add("You don`t have permission", new Dictionary<Languages, string>
        {
            { Languages.UA, "У вас немає дозволу" }, 
        });
        
        ErrorMessages.Add("Not relevant", new Dictionary<Languages, string>
        {
            { Languages.UA, "Неправильні дані" }, 
        });
        
        ErrorMessages.Add("Error to sending email", new Dictionary<Languages, string>
        {
            { Languages.UA, "Помилка надсилання листа" }, 
        });
        
        ErrorMessages.Add("Already exists", new Dictionary<Languages, string>
        {
            { Languages.UA, "Вже існує" }, 
        });
        
        ErrorMessages.Add("Achieved max", new Dictionary<Languages, string>
        {
            { Languages.UA, "Досягнутий максимум" }, 
        });
        
        ErrorMessages.Add("Does not meet the requirements", new Dictionary<Languages, string>
        {
            { Languages.UA, "Не відповідає вимогам" }, 
        });
        
        ErrorMessages.Add("Owner cann`t leave not empty class group", new Dictionary<Languages, string>
        {
            { Languages.UA, "Власник не може залишити не порожню групу" }, 
        });
        
        ErrorMessages.Add("University not supported", new Dictionary<Languages, string>
        {
            { Languages.UA, "Університет не підтримується" }, 
        });
        
        ErrorMessages.Add("Invalid email or password", new Dictionary<Languages, string>
        {
            { Languages.UA, "Неправильна адреса електронної пошти або пароль" }, 
        });
        
        ErrorMessages.Add("Invalid tokens", new Dictionary<Languages, string>
        {
            { Languages.UA, "Недійсні токени" }, 
        });
        
        ErrorMessages.Add("Information already set", new Dictionary<Languages, string>
        {
            { Languages.UA, "Інформація вже встановлена" }, 
        });
        
        ErrorMessages.Add("Not found", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані не знайдено" }, 
        });
        ErrorMessages.Add("Not found user", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані про користувача не знайдено" }, 
        });
        ErrorMessages.Add("Not found class group", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані про групу не знайдено" }, 
        });
        ErrorMessages.Add("Not found subject in time table", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані про розклад не знайдено" }, 
        });
        ErrorMessages.Add("Not found student invitation", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані про запрошення не знайдено" }, 
        });
        ErrorMessages.Add("Not found subject", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані про предмет не знайдено" }, 
        });
        ErrorMessages.Add("Not found lecturer", new Dictionary<Languages, string>
        {
            { Languages.UA, "Дані про викладача не знайдено" }, 
        });
    }

    public static string GetUaDisplayMessage(string message)
    {
        if (ErrorMessages.TryGetValue(message, out var displayMessages) && displayMessages.TryGetValue(Languages.UA, out var displayMessage))
            message = displayMessage;
        
        return message;
    }
}