using Core;

namespace UI.Factories
{
    /// <summary>
    /// Factory to push <see cref="EmployeeInsertOrEditWindow"/> service dependencies.
    /// </summary>
    public interface IEmployeeInsertOrEditWindowFactory
    {
        EmployeeInsertOrEditWindow Create(IEmployee? employee = null);
    }
}
