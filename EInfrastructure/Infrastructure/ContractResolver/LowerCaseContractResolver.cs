using Newtonsoft.Json.Serialization;

namespace EInfrastructure.Infrastructure.ContractResolver
{
    /// <summary>
    /// 参数全部小写
    /// </summary>
    public class LowerCaseContractResolver: DefaultContractResolver
    {
        /// <inheritdoc />
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}