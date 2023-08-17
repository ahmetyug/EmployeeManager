using Core;
using System.Text.Json.Serialization;

namespace Services.EmployeeProviders.GorestV2
{
    public partial class GorestV2EmployeeProvider : IEmployeeProvider
    {
        /// <summary>
        /// Provider specific <see cref="IEmployee"/> implementation.
        /// It is private so that the type never gets exposed.
        /// </summary>
        private class GorestV2Employee : IEmployee
        {
            [JsonPropertyName("gender")]
            public string GenderS { get; init; }

            [JsonPropertyName("status")]
            public string StatusS { get; init; }

            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("email")]
            public string Email { get; init; }

            [JsonIgnore]
            public Gender Gender => GenderS == "male" ? Gender.Male : Gender.Female;

            [JsonIgnore]
            public EmployeeStatus Status => StatusS == "active" ? EmployeeStatus.Active : EmployeeStatus.InActive;

            public void Validate()
            {
                // 3rd party api model => does nothing for now
            }
        }
    }
}