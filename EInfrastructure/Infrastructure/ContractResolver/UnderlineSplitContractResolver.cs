using System.Text;
using Newtonsoft.Json.Serialization;

namespace EInfrastructure.Infrastructure.ContractResolver
{
    /// <summary>
    /// 下划线约束
    /// </summary>
    public class UnderlineSplitContractResolver: DefaultContractResolver
    {
        /// <inheritdoc />
        protected override string ResolvePropertyName(string propertyName)
        {
            return CamelCaseToUnderlineSplit(propertyName);
        }

        private string CamelCaseToUnderlineSplit(string name)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                var ch = name[i];
                if (char.IsUpper(ch) && i > 0)
                {
                    var prev = name[i - 1];
                    if (prev != '_')
                    {
                        if (char.IsUpper(prev))
                        {
                            if (i < name.Length - 1)
                            {
                                var next = name[i + 1];
                                if (char.IsLower(next))
                                {
                                    builder.Append('_');
                                }
                            }
                        }
                        else
                        {
                            builder.Append('_');
                        }
                    }
                }
                builder.Append(char.ToLower(ch));
            }

            return builder.ToString();
        }
    }
}