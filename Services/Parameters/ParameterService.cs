using Services.Parameters.ParameterProviders;

namespace Services.Parameters
{
    public class ParameterService : IParameterService
    {
        private readonly IParameterProvider parameterProvider;

        private IReadOnlyDictionary<string, object> parameters;

        public ParameterService(IParameterProvider parameterProvider) 
        {
            this.parameterProvider = parameterProvider;
        }

        public object GetParameterByKey(string key)
        {
            // Better implementation would be updating the internal parameters dictionary by a background service periodically
            this.parameters ??= this.parameterProvider.GetParameters();

            return this.parameters[key];
        }
    }
}
