using System.Runtime.Serialization;

namespace project_backend.Utils
{
    public static class EnumUtils
    {
        public static string GetEnumMemberValue(this Enum value)
        {
            var type = value.GetType();

            var memberInfo = type.GetMember(value.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                if (memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault() is EnumMemberAttribute enumValueMember)
                {
                    return enumValueMember.Value;
                }
            }

            return value.ToString();
        }
    }
}
