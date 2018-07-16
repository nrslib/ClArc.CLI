using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MockUseCase.Lib.JsonResponse
{
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jProperty = base.CreateProperty(member, memberSerialization);
            if (jProperty.Writable)
            {
                return jProperty;
            }

            jProperty.Writable = member.IsPropertyWithSetter();

            return jProperty;
        }
    }

    internal static class MemberInfoExtentions
    {
        internal static bool IsPropertyWithSetter(this MemberInfo member)
        {
            var prop = member as PropertyInfo;

            return prop?.GetSetMethod(true) != null;
        }
    }
}
