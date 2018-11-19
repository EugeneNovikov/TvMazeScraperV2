namespace Shared.Wrappers
{
    public interface IJsonConverterWrapper
    {
        T DeserializeObject<T>(string obj);

        T DeserializeObjectSafe<T>(string obj);

        string SerializeObject(object obj);
    }
}