namespace FinancialCalc.Interfaces
{
    public interface IJsonService
    {
        bool TryDeserializeObject<T>(string value, out T deserializedObject) where T : class;

        bool TrySerializeObject(object objectToSerialize, out string data);
    }
}
