namespace Services.Parameters
{
    /// <summary>
    /// A service for application parameter operations.
    /// </summary>
    public interface IParameterService
    {
        object GetParameterByKey(string key);
    }
}
