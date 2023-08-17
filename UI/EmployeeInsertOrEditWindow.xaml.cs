using Core;
using Services.EmployeeProviders;
using System;
using System.Windows;
using System.Windows.Controls;
using UI.Models;

namespace UI
{
    /// <summary>
    /// Interaction logic for EmployeeInsertOrEditWindow.xaml
    /// </summary>
    public partial class EmployeeInsertOrEditWindow : Window
    {
        private readonly IEmployeeProvider employeeProvider;

        private readonly bool isInsertMode;

        public EmployeeInsertOrEditWindow(IEmployeeProvider employeeProvider, IEmployee? employee = null)
        {
            InitializeComponent();

            this.employeeProvider = employeeProvider;

            isInsertMode = employee == null;

            SetUITexts(isInsertMode);

            this.employeeControlsContainer.DataContext = isInsertMode ? new EmployeeInsertOrUpdateModel() : new EmployeeInsertOrUpdateModel(employee);
        }

        private async void btnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as EmployeeInsertOrUpdateModel;

            try
            {
                employee.Validate();
            }
            catch (Exception ex)
            {
                var message = $"Some of the fields are invalid:{Environment.NewLine}" +
                    $"{ex.Message}";
                MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (isInsertMode)
            {
                var employeeResult = await this.employeeProvider.InsertAsync(employee);

                if (!employeeResult.IsSuccess)
                {
                    var message = $"Error while attempting to create a new employee:{Environment.NewLine}" +
                        $"{employeeResult.Message}";
                    MessageBox.Show(message, "Employee Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var message = "Employee is successfully created";
                    MessageBox.Show(message, "Successfully Created", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                var employeeResult = await this.employeeProvider.UpdateAsync(employee);

                if (!employeeResult.IsSuccess)
                {
                    var message = $"Error while attempting to update employee:{Environment.NewLine}" + 
                        $"{employeeResult.Message}";
                    MessageBox.Show(message, "Employee Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var message = "Employee is successfully updated";
                    MessageBox.Show(message, "Successfully Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void SetUITexts(bool isInsert)
        {
            if (isInsert)
            {
                this.Title = "Create Employee";
            }
            else
            {
                this.Title = "Edit Employee";
            }
            
            this.employeeDetailsContainer.Header = this.Title;
        }
    }
}
