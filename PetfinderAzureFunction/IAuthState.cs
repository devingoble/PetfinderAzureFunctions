namespace PetfinderAzureFunction
{
    public interface IAuthState
    {
        string AccessToken { get; set; }

        string Get();
        void Set(string token);
    }
}