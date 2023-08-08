using KitchenLib.JSON.Enums;
using KitchenLib.JSON.Models.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
	public class ViewConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);

			

			if (jObject.TryGetValue("Type", out JToken type))
			{
				return type.ToObject<ViewType>() switch
				{
					ViewType.ItemGroupView => jObject.ToObject<ItemGroupViewContainer>(),
					ViewType.ObjectsSplittableView => jObject.ToObject<ObjectsSplittableViewContainer>(),
					ViewType.PositionSplittableView => jObject.ToObject<PositionSplittableViewContainer>(),
					_ => throw new NotImplementedException(),
				};
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{

		}
	}
}
