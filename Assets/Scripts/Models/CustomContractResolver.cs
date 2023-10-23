using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using UnityEngine;

public class CustomContractResolver : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (objectType == typeof(Vector3))
        {
            return new Vector3Converter();
        }
        return base.ResolveContractConverter(objectType);
    }
}
