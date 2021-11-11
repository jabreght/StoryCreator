using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class KeyValuePair
{
	public string key;
	public string value;

	public KeyValuePair()
	{
		key = "";
		value = "";
	}

	public KeyValuePair(string k, string v)
	{
		key = k;
		value = v;
	}

	public static implicit operator JSONObject(KeyValuePair value)
	{
		JSONObject sObject = new JSONObject();

		sObject["Variable"] = value.key;
		sObject["Value"] = value.value;

		return sObject;
	}
	public static explicit operator KeyValuePair(JSONNode value)
	{
		KeyValuePair data = new KeyValuePair();

		data.key = value["Variable"];
		data.value = value["Value"];

		return data;
	}

	public void Print()
	{
		Debug.Log(key);
		Debug.Log(value);
	}
}

[System.Serializable]
public class KeyValueContainer
{
	public List<KeyValuePair> items;
}