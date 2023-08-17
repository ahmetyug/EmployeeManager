using Core;

namespace Services.EmployeeProviders
{
    /// <summary>
    /// Interface which is responsible for transactions regarding <see cref="IEmployee"/>s.
    /// </summary>
    public interface IEmployeeProvider
    {
        Task<TxResult<IEmployee>> GetAsync(int id);

        Task<TxResult<IReadOnlyList<IEmployee>>> GetAllAsync(string? name = null, int? pageNumbner = null);

        Task<TxResult<IEmployee>> DeleteAsync(IEmployee employee);

        Task<TxResult<IEmployee>> InsertAsync(IEmployee employee);

        Task<TxResult<IEmployee>> UpdateAsync(IEmployee employee);
    }
}
