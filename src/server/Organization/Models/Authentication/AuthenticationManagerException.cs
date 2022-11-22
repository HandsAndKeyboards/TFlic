namespace Organization.Models.Authentication;

public class AuthenticationManagerException : Exception
{
    public AuthenticationManagerException(string message) : base(message) { }
    public AuthenticationManagerException() { }
}