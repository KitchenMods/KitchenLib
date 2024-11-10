using Newtonsoft.Json;
using System.Collections.Generic;

namespace KitchenLib.Preferences
{
	public abstract class PreferenceBase
	{
		public string Key { get; protected set; }

		internal PreferenceBase(string key)
		{
			Key = key;
		}

		public abstract string Serialize();

		public abstract void Deserialize(string json);

		public abstract void Reset();
	}

	public abstract class PreferenceBase<T> : PreferenceBase
	{
		public T Value { get; protected set; }
		private T DefaultValue;

		public PreferenceBase(string key, T defaultValue = default) : base(key)
		{
			DefaultValue = defaultValue;
			Value = defaultValue;
		}

		public void Set(T value)
		{
			Value = value;
		}

		public T Get()
		{
			return Value;
		}

		public override void Reset()
		{ 
			Value = DefaultValue;
		}
	}

	public class PreferenceBool : PreferenceBase<bool>
	{
		public PreferenceBool(string key, bool defaultValue = false) : base(key, defaultValue) { }

		public override string Serialize()
		{
			return Value.ToString();
		}

		public override void Deserialize(string json)
		{
			Value = bool.Parse(json);
		}
	}

	public class PreferenceString : PreferenceBase<string>
	{
		public PreferenceString(string key, string defaultValue = null) : base(key, defaultValue) { }

		public override string Serialize()
		{
			return Value.ToString();
		}

		public override void Deserialize(string json)
		{
			Value = json;
		}
	}

	public class PreferenceInt : PreferenceBase<int>
	{
		public PreferenceInt(string key, int defaultValue = 0) : base(key, defaultValue) { }

		public override string Serialize()
		{
			return Value.ToString();
		}

		public override void Deserialize(string json)
		{
			Value = int.Parse(json);
		}
	}

	public class PreferenceFloat : PreferenceBase<float>
	{
		public PreferenceFloat(string key, float defaultValue = 0) : base(key, defaultValue) { }
		public override string Serialize()
		{
			return Value.ToString();
		}
		public override void Deserialize(string json)
		{
			Value = float.Parse(json);
		}
	}

	public class PreferenceList<T> : PreferenceBase<List<T>>
	{
		public PreferenceList(string key, List<T> defaultValue = null) : base(key, defaultValue) { }
		public override string Serialize()
		{
			return JsonConvert.SerializeObject(Value);
		}
		public override void Deserialize(string json)
		{
			Value = JsonConvert.DeserializeObject<List<T>>(json);
		}
	}

	public class PreferenceDictionary<T1, T2> : PreferenceBase<Dictionary<T1, T2>>
	{
		public PreferenceDictionary(string key, Dictionary<T1, T2> defaultValue = null) : base(key, defaultValue) { }
		public override string Serialize()
		{
			return JsonConvert.SerializeObject(Value);
		}
		public override void Deserialize(string json)
		{
			Value = JsonConvert.DeserializeObject<Dictionary<T1, T2>>(json);
		}
	}
}
