using Core;
using System;
using Utility;

namespace UI.Models
{
    /// <summary>
    /// Insert or update specific implementation of <see cref="IEmployee"/>.
    /// </summary>
    internal class EmployeeInsertOrUpdateModel : IEmployee
    {
        public int Id { get; } = 0;

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsMale { get; set; } = false;

        public bool IsFemale { get; set; } = false;

        public bool IsActive { get; set; } = false;

        public bool IsInActive { get; set; } = false;

        public Gender Gender => IsMale ? Gender.Male : Gender.Female;

        public EmployeeStatus Status => IsActive ? EmployeeStatus.Active : EmployeeStatus.InActive;

        public EmployeeInsertOrUpdateModel() 
        {
        }

        public EmployeeInsertOrUpdateModel(IEmployee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            Email = employee.Email;
            IsMale = employee.Gender == Gender.Male;
            IsFemale = employee.Gender == Gender.Female;
            IsActive = employee.Status == EmployeeStatus.Active;
            IsInActive = employee.Status == EmployeeStatus.InActive;
        }

        public void Validate()
        {
            if (!this.Name.ValidateString(allowNullOrWhiteSpace: false, minLength: 3))
            {
                throw new ArgumentException($"<{this.Name}> is invalid for {nameof(this.Name)}", nameof(this.Name));
            }

            if (!this.Email.ValidateAsEmail(allowNullOrWhiteSpace: false))
            {
                throw new ArgumentException($"<{this.Email}> is invalid for {nameof(this.Email)}", nameof(this.Email));
            }

            if (!(new bool[] { this.IsMale, this.IsFemale }).ValidateBoolGroup(shouldContainOneTrue: true))
            {
                throw new ArgumentException($"Only one of {nameof(this.IsMale)} and {nameof(this.IsFemale)} should be true", nameof(this.IsMale));
            }

            if (!(new bool[] { this.IsActive, this.IsInActive }).ValidateBoolGroup(shouldContainOneTrue: true))
            {
                throw new ArgumentException($"Only one of {nameof(this.IsActive)} and {nameof(this.IsInActive)} should be true", nameof(this.IsActive));
            }
        }

    }
}
