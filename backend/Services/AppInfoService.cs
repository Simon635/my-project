namespace backend.Services;

public class AppInfoService : IAppInfoService
{
    public string GetApplicationName()
    {
        return "Multimedia Repair API";
    }
}
