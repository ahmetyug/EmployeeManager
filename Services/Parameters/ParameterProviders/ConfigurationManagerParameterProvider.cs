using System.Configuration;

namespace Services.Parameters.ParameterProviders
{
    /// <summary>
    /// Specific <see cref="IParameterProvider"/> which gets parameters from ConfigurationManagerParameterProvider.
    /// </summary>
    public class ConfigurationManagerParameterProvider : IParameterProvider
    {
        private readonly IReadOnlyDictionary<string, object> parameters;

        public ConfigurationManagerParameterProvider()
        {
            this.parameters = RetrieveParameters();
        }

        public IReadOnlyDictionary<string, object> GetParameters()
        {
            // reading only once and returning the same parameters are fine for ConfigurationManager
            return this.parameters;
        }

        private IReadOnlyDictionary<string, object> RetrieveParameters()
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("GORESTV2_API_BASE_URL", ConfigurationManager.AppSettings["GorestV2ApiBaseUrl"]);
            parameters.Add("GORESTV2_API_TOKEN", ConfigurationManager.AppSettings["GorestV2ApiToken"]);

            return parameters;
        }
    }
}
