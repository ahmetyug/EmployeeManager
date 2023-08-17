using Core;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using Services.Parameters;

namespace Services.EmployeeProviders.GorestV2
{
    /// <summary>
    /// One of potentially many employee providers.
    /// </summary>
    public partial class GorestV2EmployeeProvider : IEmployeeProvider
    {
        private readonly IParameterService parameterService;

        public GorestV2EmployeeProvider(IParameterService parameterService)
        {
            this.parameterService = parameterService;
        }

        public async Task<TxResult<IEmployee>> GetAsync(int id)
        {
            try
            {
                var request = new RestRequest(id.ToString(), Method.Get);
                request.AddHeader("authorization", $"Bearer {GetApiToken()}");

                var result = await CreateRestClient().GetAsync<GorestV2Employee>(request);

                if (result.Id == 0)
                {
                    return TxResult<IEmployee>.OfFail(TxCode.Fail, "Employee is not found");
                }

                return TxResult<IEmployee>.OfSuccess(result);
            }
            catch (Exception e)
            {
                return TxResult<IEmployee>.OfFail(TxCode.Fail, e.Message);
            }
        }

        public async Task<TxResult<IReadOnlyList<IEmployee>>> GetAllAsync(string? name = null, int? pageNumber = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (name != null)
                {
                    queryParams.Add($"name={name}");
                }
                if (pageNumber != null)
                {
                    queryParams.Add($"page={pageNumber}");
                }
                var queryParam = queryParams.Any() ? $"?{string.Join("&", queryParams)}" : null;

                var request = new RestRequest(queryParam, Method.Get);
                request.AddHeader("authorization", $"Bearer {GetApiToken()}");

                var result = await CreateRestClient().GetAsync<IList<GorestV2Employee>>(request);
                return TxResult<IReadOnlyList<IEmployee>>.OfSuccess(result.Cast<IEmployee>().ToList());
            }
            catch (Exception e)
            {
                return TxResult<IReadOnlyList<IEmployee>>.OfFail(TxCode.Fail, e.Message);
            }
        }

        public async Task<TxResult<IEmployee>> InsertAsync(IEmployee employee)
        {
            try
            {
                var request = new RestRequest()
                {
                    Method = Method.Post
                };

                request.AddHeader("authorization", $"Bearer {GetApiToken()}");
                request.AddHeader("Content-type", "application/json");

                request.AddJsonBody(new
                {
                    email = employee.Email,
                    name = employee.Name,
                    status = employee.Status == EmployeeStatus.Active ? "active" : "inactive",
                    gender = employee.Gender == Gender.Male ? "male" : "female",
                });

                var result = await CreateRestClient().PostAsync(request);

                if (!result.IsSuccessful)
                {
                    return TxResult<IEmployee>.OfFail(TxCode.Fail, result.ErrorMessage ?? result.Content ?? "Error While Inserting..");
                }

                var insertedEmployee = JsonSerializer.Deserialize<GorestV2Employee>(result.Content);
                return TxResult<IEmployee>.OfSuccess(insertedEmployee);
            }
            catch (Exception e)
            {
                return TxResult<IEmployee>.OfFail(TxCode.Fail, e.Message);
            }
        }

        public async Task<TxResult<IEmployee>> UpdateAsync(IEmployee employee)
        {
            try
            {
                var request = new RestRequest($"/{employee.Id}", Method.Put);
                request.AddHeader("authorization", $"Bearer {GetApiToken()}");
                request.AddHeader("Content-type", "application/json");

                request.AddJsonBody(new
                {
                    id = employee.Id,
                    email = employee.Email,
                    name = employee.Name,
                    status = employee.Status == EmployeeStatus.Active ? "active" : "inactive",
                    gender = employee.Gender == Gender.Male ? "male" : "female"
                });

                var result = await CreateRestClient().PutAsync<GorestV2Employee>(request);
                return TxResult<IEmployee>.OfSuccess(result);
            }
            catch (Exception e)
            {
                return TxResult<IEmployee>.OfFail(TxCode.Fail, e.Message);
            }
        }

        public async Task<TxResult<IEmployee>> DeleteAsync(IEmployee employee)
        {
            try
            {
                var request = new RestRequest($"/{employee.Id}", Method.Delete);
                request.AddHeader("authorization", $"Bearer {GetApiToken()}");

                var result = await CreateRestClient().DeleteAsync(request);

                if (!result.IsSuccessful)
                {
                    return TxResult<IEmployee>.OfFail(TxCode.Fail, result.ErrorMessage ?? result.Content ?? "Error While Deleting..");
                }
                return TxResult<IEmployee>.OfSuccess(employee);
            }
            catch (Exception e)
            {
                return TxResult<IEmployee>.OfFail(TxCode.Fail, e.Message);
            }
        }

        private string GetApiToken()
        {
            return this.parameterService.GetParameterByKey("GORESTV2_API_TOKEN").ToString();
        }

        private RestClient CreateRestClient()
        {
            var apiBaseUrl = this.parameterService.GetParameterByKey("GORESTV2_API_BASE_URL").ToString();

            return new RestClient(apiBaseUrl,
                 configureSerialization: s => s.UseSystemTextJson(new System.Text.Json.JsonSerializerOptions()
                 {
                 }));
        }
    }
}