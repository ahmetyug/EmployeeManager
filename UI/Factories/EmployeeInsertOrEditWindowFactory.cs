using Core;
using Services.EmployeeProviders;

namespace UI.Factories
{
    internal class EmployeeInsertOrEditWindowFactory : IEmployeeInsertOrEditWindowFactory
    {
        private readonly IEmployeeProvider employeeProvider;

        public EmployeeInsertOrEditWindowFactory(IEmployeeProvider employeeProvider)
        {
            this.employeeProvider = employeeProvider;
        }

        public EmployeeInsertOrEditWindow Create(IEmployee? employee = null)
        {
            return new EmployeeInsertOrEditWindow(this.employeeProvider, employee);
        }
    }
}
