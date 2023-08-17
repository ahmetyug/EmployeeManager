using Core;
using Services.EmployeeProviders;
using Services.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Factories;
using Utility;
using WinForms = System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEmployeeProvider employeeProvider;
        private readonly IReportingService reportingService;
        private readonly IEmployeeInsertOrEditWindowFactory employeeInsertOrEditWindowFactory;

        private int currentPageNumber = 1;
        private string? lastSearchText = string.Empty;

        public static IReadOnlyList<string> genders = new List<string>() { "male", "female" };

        public MainWindow(IEmployeeProvider employeeProvider, IReportingService reportingService, IEmployeeInsertOrEditWindowFactory employeeInsertOrEditWindowFactory)
        {
            InitializeComponent();

            this.employeeProvider = employeeProvider;
            this.reportingService = reportingService;
            this.employeeInsertOrEditWindowFactory = employeeInsertOrEditWindowFactory;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchText = this.textBoxSearch.Text.GetDefaultIfNullOrWhiteSpace(null);
            var requestPageNumber = 1;

            await searchAndRenderEmployees(searchText, requestPageNumber);
        }

        private async void textBoxSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var searchText = this.textBoxSearch.Text.GetDefaultIfNullOrWhiteSpace(null);
                var requestPageNumber = 1;

                await searchAndRenderEmployees(searchText, requestPageNumber);
            }
        }

        private void btnCreateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var insertWindow = this.employeeInsertOrEditWindowFactory.Create();
            insertWindow.ShowDialog();
        }

        private async void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            var searchText = this.lastSearchText;
            var requestPageNumber = this.currentPageNumber <= 1 ? 1 : this.currentPageNumber - 1;

            await searchAndRenderEmployees(searchText, requestPageNumber);
        }

        private async void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            var searchText = this.lastSearchText;
            var requestPageNumber = this.currentPageNumber + 1;

            await searchAndRenderEmployees(searchText, requestPageNumber);
        }

        private async void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as IEmployee;

            var employeeResult = await this.employeeProvider.GetAsync(employee.Id);

            if (!employeeResult.IsSuccess)
            {
                var message = $"Error while attempting to get employee details:{Environment.NewLine}" +
                    $"{employeeResult.Message}";
                MessageBox.Show(message, "Employee Details Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var editWindow = this.employeeInsertOrEditWindowFactory.Create(employeeResult.Data);
                editWindow.ShowDialog();

                await searchAndRenderEmployeesByLastConfiguration();
            }
        }

        private async void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as IEmployee;

            var message = $"You are about to delete the following employee:{Environment.NewLine}{Environment.NewLine}" +
                $"{nameof(IEmployee.Name)}: {employee.Name}{Environment.NewLine}" +
                $"{nameof(IEmployee.Email)}: {employee.Email}{Environment.NewLine}" +
                $"{nameof(IEmployee.Gender)}: {employee.Gender}{Environment.NewLine}" +
                $"{nameof(IEmployee.Status)}: {employee.Status}{Environment.NewLine}{Environment.NewLine}" +
                $"Are you sure?";

            var msgBoxDeleteResult = MessageBox.Show(message, "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (msgBoxDeleteResult == MessageBoxResult.Yes)
            {
                var deleteResult = await this.employeeProvider.DeleteAsync(employee);

                if (!deleteResult.IsSuccess)
                {
                    var deleteResultMessage = $"Error while attempting to delete an employee:{Environment.NewLine}" +
                        $"{deleteResult.Message}";
                    MessageBox.Show(deleteResultMessage, "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var deleteResultMessage = "Employee is successfully deleted";
                    MessageBox.Show(deleteResultMessage, "Successfully Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                    await searchAndRenderEmployeesByLastConfiguration();
                }
            }
        }

        private async void btnGenerateCsv_Click(object sender, RoutedEventArgs e)
        {
            var employees = (sender as Button).DataContext as IReadOnlyList<IEmployee>;

            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();

            var dialogResult = dialog.ShowDialog();

            if (dialogResult == WinForms.DialogResult.OK)
            {
                string dirPath = dialog.SelectedPath;

                try
                {
                    var file = await this.reportingService.GenerateReportAsync(employees, ReportType.CSV, dirPath);

                    if (file.Exists)
                    {
                        var message = $"Csv file is created:{Environment.NewLine}" +
                            $"{file.FullName}";
                        MessageBox.Show(message, "CSV Generated", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var message = $"Error while attempting to create a csv file:";
                        MessageBox.Show(message, "Generate CSV Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    var message = $"Error while attempting to create a csv file:{Environment.NewLine}" +
                        $"{ex.Message}";
                    MessageBox.Show(message, "Generate CSV Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task searchAndRenderEmployeesByLastConfiguration()
        {
            var searchText = this.lastSearchText;
            var requestPageNumber = this.currentPageNumber;

            await searchAndRenderEmployees(searchText, requestPageNumber);
        }

        private async Task searchAndRenderEmployees(string? searchText, int pageNumber)
        {
            var employeesResult = await this.employeeProvider.GetAllAsync(searchText, pageNumber);

            if (!employeesResult.IsSuccess)
            {
                var message = $"Error while attempting to search employees:{Environment.NewLine}" +
                    $"{employeesResult.Message}";
                MessageBox.Show(message, "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                renderEmployees(employeesResult.Data, searchText, pageNumber);
            }
        }

        private void renderEmployees(IReadOnlyList<IEmployee> employees, string? searchText, int pageNumber)
        {
            this.listEmployees.ItemsSource = employees;

            this.btnGenerateCsv.DataContext = employees;
            this.btnGenerateCsv.Visibility = employees.Any() ? Visibility.Visible : Visibility.Hidden;

            this.lblCurrentPage.Content = pageNumber.ToString();
            this.btnPrevPage.Visibility = pageNumber > 1 ? Visibility.Visible : Visibility.Hidden;
            this.btnNextPage.Visibility = employees.Any() ? Visibility.Visible : Visibility.Hidden;

            this.tablePagenation.Visibility = Visibility.Visible;

            this.currentPageNumber = pageNumber;

            this.lastSearchText = searchText;
        }
    }
}
