namespace Services.Parameters.ParameterProviders
{
    /// <summary>
    /// Interface for parameter retrievel operations.
    /// </summary>
    public interface IParameterProvider
    {
        IReadOnlyDictionary<string, object> GetParameters();
    }
}
