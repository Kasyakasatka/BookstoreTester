using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BookstoreTester.Mvc.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumValue) where T :  Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

        return attribute?.Description ?? enumValue.ToString();
    }
    public static T GetEnumValueFromDescription<T>(string description) where T : Enum
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new ArgumentException("T must be an Enum type.", nameof(T));

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute?.Description == description)
            {
                return (T)field.GetValue(null);
            }
        }
        return default(T);
    }
}