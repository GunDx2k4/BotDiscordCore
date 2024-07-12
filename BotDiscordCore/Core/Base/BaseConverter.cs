using Newtonsoft.Json;

namespace BotDiscordCore.Core.Base
{
    public abstract class BaseConverter<T> : JsonConverter where T : struct
    {
        private readonly bool _quotes;

        public BaseConverter(bool useQuotes)
        {
            _quotes = useQuotes;
        }

        protected abstract List<KeyValuePair<T, string>> Mapping { get; }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T) || Nullable.GetUnderlyingType(objectType) == typeof(T);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stringValue = value == null ? null : GetValue((T)value);
            if (_quotes) writer.WriteValue(stringValue);
            else writer.WriteRawValue(stringValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            var stringValue = reader.Value.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (!GetValue(stringValue, out var result))
            {
                return null;
            }

            return result;
        }

        public T ReadString(string data)
        {
            return Mapping.FirstOrDefault(v => v.Value == data).Key;
        }

        private bool GetValue(string value, out T result)
        {
            var mapping = Mapping.FirstOrDefault(kv => kv.Value.Equals(value, StringComparison.InvariantCulture));
            if (mapping.Equals(default(KeyValuePair<T, string>)))
                mapping = Mapping.FirstOrDefault(kv => kv.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));

            if (!mapping.Equals(default(KeyValuePair<T, string>)))
            {
                result = mapping.Key;
                return true;
            }

            result = default;
            return false;
        }

        private string GetValue(T value)
        {
            return Mapping.FirstOrDefault(v => v.Key.Equals(value)).Value;
        }
    }
}
