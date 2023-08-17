namespace Core
{
    /// <summary>
    /// Main employee interface which will be passed between service and UI
    /// </summary>
    public interface IEmployee : IValidatableObject
    {
        int Id { get; }

        string Name { get; }

        string Email { get; }

        Gender Gender { get; }

        EmployeeStatus Status { get; }
    }
}
