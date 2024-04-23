namespace MenCore.Mailing;

public class ToEmail
{
    public ToEmail()
    {
    }

    public ToEmail(string email, string fullName)
    {
        Email = email;
        FullName = fullName;
    }

    public string Email { get; set; }
    public string FullName { get; set; }
}