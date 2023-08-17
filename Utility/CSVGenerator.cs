
using System.Reflection;
using System.Text;

namespace Utility
{
    public static class CSVGenerator
    {
        public static StringBuilder GenerateCsv<T>(IReadOnlyList<T> dataList)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && (!p.PropertyType.IsClass || p.PropertyType == typeof(String)))
                .Select(p => p.Name).ToList();

            var csv = new StringBuilder();

            var headerRow = string.Join(",", properties);
            csv.AppendLine(headerRow);

            foreach (var data in dataList)
            {
                var values = new List<string>();

                foreach (var property in properties) 
                {
                    var propertyValue = data.GetType().GetProperty(property).GetValue(data, null);
                    values.Add(propertyValue.ToString());
                }

                var currentRow = string.Join(",", values);
                csv.AppendLine(currentRow);
            }


            return csv;
        }
    }
}
