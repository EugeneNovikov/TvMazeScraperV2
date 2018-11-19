using Newtonsoft.Json;

namespace Shared.Wrappers
{
    public class JsonConverterWrapper : IJsonConverterWrapper
    {
        private static readonly JsonSerializerSettings SafeSerializerSettings = new JsonSerializerSettings
        {
            Error = (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            }
        };

        public T DeserializeObject<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public T DeserializeObjectSafe<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj, SafeSerializerSettings);
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
