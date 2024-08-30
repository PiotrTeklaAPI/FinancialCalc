﻿using System.Collections.Generic;

namespace FinancialCalc.Interfaces
{
    public interface IJsonService
    {
        bool TryDeserializeObject<T>(string value, out List<T> deserializedObject) where T : class;

        bool TrySerializeObject(object objectToSerialize, out string data);
    }
}
