using Swappa.Entities.Enums;
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

        public static bool IsInEnum<TEnum>(this object value) where TEnum : Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
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
            @enum = default!;
            if (Enum.TryParse(typeof(TEnum), value, out object? result))
            {
                @enum = (TEnum)result!;
                return true;
            }
            return false;
        }

        public static bool IsInOneOrMoreRoles(this List<SystemRole> roles, params SystemRole[] systemRoles)
        {
            return roles.Any(systemRoles.Contains);
        }

        public static List<TEnum> ParseValues<TEnum>(this List<string> values) where TEnum : Enum
        {
            var result = new List<TEnum>();
            values.ForEach(e =>
            {
                if(Enum.TryParse(typeof(TEnum), e, true, out object? output))
                {
                    result.Add((TEnum)output!);
                }
            });
            
            return result;
        }

        public static List<TEnum> ParseValues<TEnum>(this List<int> values) where TEnum : Enum
        {
            var result = new List<TEnum>();
            values.ForEach(e =>
            {
                if (e.TryParseValue<TEnum>(out var @enum))
                {
                    result.Add(@enum);
                }
            });

            return result;
        }

        public static bool TryParseValue<TEnum>(this int target, out TEnum @enum) where TEnum : Enum
        {
            @enum = default!;
            if (!target.IsInEnum<TEnum>()) return false;
            @enum = (TEnum)Enum.ToObject(typeof(TEnum), target);
            return true;
        }

        public static List<string> AllRolesString()
        {
            return new List<string>
            {
                SystemRole.Admin.ToString(),
                SystemRole.Merchant.ToString(),
                SystemRole.SuperAdmin.ToString(),
                SystemRole.User.ToString()
            };
        }
    }
}
