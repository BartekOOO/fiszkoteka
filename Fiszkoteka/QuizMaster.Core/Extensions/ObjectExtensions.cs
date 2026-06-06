using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuizMaster.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToPrettyString(this object target)
        {
            if (target == null)
                return "<null>";

            var sb = new StringBuilder();

            Type type = target.GetType();

            sb.AppendLine(type.Name);
            sb.AppendLine(new string('-', type.Name.Length));

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object value;

                try
                {
                    value = prop.GetValue(target, null);
                }
                catch (Exception ex)
                {
                    value = $"<błąd odczytu: {ex.Message}>";
                }

                sb.Append(prop.Name);
                sb.Append(": ");
                sb.AppendLine(FormatValue(value));
            }

            return sb.ToString();
        }

        private static string FormatValue(object value)
        {
            if (value == null)
                return "<null>";

            if (value is string)
                return value.ToString();

            if (value is DateTime dateTime)
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            if (value is IEnumerable enumerable)
            {
                var items = enumerable
                    .Cast<object>()
                    .Select(x => x == null ? "<null>" : x.ToString())
                    .ToList();

                return "[" + string.Join(", ", items) + "]";
            }

            return value.ToString();
        }
    }
}
