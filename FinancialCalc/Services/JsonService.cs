using FinancialCalc.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FinancialCalc.Services
{
    public class JsonService : IJsonService
    {
        public bool TryDeserializeObject<T>(string value, out T deserializedObject) where T : class
        {
            bool resut;
            deserializedObject = null;

            try
            {
                deserializedObject = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto});
                resut = deserializedObject != null;
            }
            catch (Exception ex)
            {
                resut = false;
            }

            return resut;
        }

        public bool TrySerializeObject(object objectToSerialize, out string data)
        {
            data = JsonConvert.SerializeObject(objectToSerialize, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (data != null && string.IsNullOrEmpty(data) is false)
            {
                return true;
            }
            return false;
        }
    }
}
