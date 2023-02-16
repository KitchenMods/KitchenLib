using Kitchen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace KitchenLib.Preferences
{
	public abstract class PreferenceBase
	{
		public string Key { get; protected set; }
		public object Value { get; protected set; }
		public string raw_value { get; set; }

		public abstract void Serialize();
		public abstract void Deserialize();
		public PreferenceBase(string key) { Key = key; }

		public void Set(object value)
		{
			Value = value;
		}
		public object Get()
		{
			return Value;
		}
	}

	public class PreferenceBool : PreferenceBase
	{
		public PreferenceBool(string key) : base(key) { }

		public override void Serialize()
		{
			raw_value = Value.ToString();
		}
		public override void Deserialize()
		{
			Value = bool.Parse(raw_value);
		}
	}

	public class PreferenceString : PreferenceBase
	{
		public PreferenceString(string key) : base(key) { }
		public override void Serialize()
		{
			raw_value = Value.ToString();
		}
		public override void Deserialize()
		{
			Value = raw_value;
		}
	}

	public class PreferenceInt : PreferenceBase
	{
		public PreferenceInt(string key) : base(key) { }
		public override void Serialize()
		{
			raw_value = Value.ToString();
		}
		public override void Deserialize()
		{
			Value = int.Parse(raw_value);
		}
	}

	public class PreferenceFloat : PreferenceBase
	{
		public PreferenceFloat(string key) : base(key) { }
		public override void Serialize()
		{
			raw_value = Value.ToString();
		}
		public override void Deserialize()
		{
			Value = float.Parse(raw_value);
		}
	}

	public class PreferenceList<T> : PreferenceBase
	{
		public PreferenceList(string key) : base(key) { }
		public override void Serialize()
		{
			string json = JsonConvert.SerializeObject(Value);
			raw_value = json;
		}
		public override void Deserialize()
		{
			Value = JsonConvert.DeserializeObject<List<T>>(raw_value);
		}
	}

	public class PreferenceDictionary<T1, T2> : PreferenceBase
	{
		public PreferenceDictionary(string key) : base(key) { }
		public override void Serialize()
		{
			string json = JsonConvert.SerializeObject(Value);
			raw_value = json;
		}
		public override void Deserialize()
		{
			Value = JsonConvert.DeserializeObject<Dictionary<T1, T2>>(raw_value);
		}
	}
}
