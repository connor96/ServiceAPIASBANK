namespace APIAsbank.Services
{
    public class UserService:IUserService
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("admin") && password.Equals("5egur1dad");
        }
    }
}
