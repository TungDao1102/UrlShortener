namespace UrlShortener.API.Commons
{
    public class EnvironmentManager : IEnvironmentManager
    {
        public void FatalError()
        {
            Environment.Exit(-1);
        }
    }
}
