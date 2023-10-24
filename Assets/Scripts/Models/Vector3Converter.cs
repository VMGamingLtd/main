using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class Vector3Converter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Vector3 vector = (Vector3)value;
        JObject obj = new()
        {
            { "x", vector.x },
            { "y", vector.y },
            { "z", vector.z }
        };
        obj.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        float x = obj.GetValue("x").Value<float>();
        float y = obj.GetValue("y").Value<float>();
        float z = obj.GetValue("z").Value<float>();
        return new Vector3(x, y, z);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Vector3);
    }
}
