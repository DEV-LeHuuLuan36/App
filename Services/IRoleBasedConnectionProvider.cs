namespace App.Services
{
    // Interface để Dependency Injection
    public interface IRoleBasedConnectionProvider
    {
        string GetConnectionString();
    }
}