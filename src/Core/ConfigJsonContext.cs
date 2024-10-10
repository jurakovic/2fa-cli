using System.Text.Json.Serialization;

namespace _2fa
{
	[JsonSerializable(typeof(Config))]
	internal partial class ConfigJsonContext : JsonSerializerContext
	{

	}
}
