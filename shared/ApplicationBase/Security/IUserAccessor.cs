namespace ApplicationBase.Security;

public interface IUserAccessor
{
    public string GetUserEmail();
    public string GetUserName();
}