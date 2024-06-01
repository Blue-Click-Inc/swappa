using System.ComponentModel;

namespace Swappa.Shared.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(value));
        }

        public static string GetDescription(this object value)
        {
            var field = value.GetType().GetField(value.ToString()!);
            if (Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(value));
        }

        public static bool IsInEnum<T>(this object value) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return true;
            }
            return false;
        }

        public static TEnum ParseValue<TEnum>(this string value) where TEnum : Enum
        {
            var result = Enum.Parse(typeof(TEnum), value);
            return (TEnum)result;
        }

        public static bool TryParseValue<TEnum>(this string value, out TEnum @enum) where TEnum : Enum
        {
            var isParsed = Enum.TryParse(typeof(TEnum), value, out object? result);
            if (isParsed)
            {
                @enum = (TEnum)result!;
            }
            else
            {
                @enum = default!;
            }
            return isParsed;
        }
    }
}
