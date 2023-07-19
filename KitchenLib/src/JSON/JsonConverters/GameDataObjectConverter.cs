using KitchenData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
	/// <summary>
	/// Custom JSON converter for GameDataObject and its subclasses.
	/// </summary>
	public class GameDataObjectConverter : JsonConverter
	{
		/// <summary>
		/// Determines whether this converter can convert the specified object type.
		/// </summary>
		/// <param name="objectType">The type of the object to convert.</param>
		/// <returns>true if the converter can convert the specified type; otherwise, false.</returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(GameDataObject)) || objectType == typeof(GameDataObject);
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The JsonReader to read from.</param>
		/// <param name="objectType">The type of the object.</param>
		/// <param name="existingValue">The existing value of the object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return reader.Value.ToString();
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The JsonWriter to write to.</param>
		/// <param name="value">The value to convert.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken jToken = JToken.FromObject(((GameDataObject)value).ID);
			jToken.WriteTo(writer);
		}
	}
}
