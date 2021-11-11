using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;
//using Newtonsoft.Json;

[System.Serializable]
public class StoryObject
{
	public string ID;
	public string type;
	public List<string> responses;
	public List<string> destinationTags;
	public List<string> commands;
	public List<KeyValuePair> variables;
	public string path;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public static implicit operator JSONObject(StoryObject value)
	{
		JSONObject sObject = new JSONObject();

		sObject["Node ID"] = value.ID;
		sObject["Node Type"] = value.type;
		for(int index = 0; index < value.responses.Count; index++)
		{
			sObject["Response choices"][index] = value.responses[index];
		}
		for(int index = 0; index < value.destinationTags.Count; index++)
		{
			sObject["Destination ID"][index] = value.destinationTags[index];
		}
		for(int index = 0; index < value.commands.Count; index++)
		{
			sObject["Commands"][index] = value.commands[index];
		}
		for(int index = 0; index < value.variables.Count; index++)
		{
			sObject["Variables"][index] = value.variables[index];
		}
		sObject["Path"] = value.path;

		return sObject;
	}


	public static explicit operator StoryObject(JSONNode value)
	{
		StoryObject data = new StoryObject();

		data.ID = value["Node ID"];
		data.type = value["Node Type"];
		foreach(var item in value["Response choices"])
		{
			data.responses.Add(item.Value);
		}
		foreach(var item in value["Destination ID"])
		{
			data.destinationTags.Add(item.Value);
		}
		foreach(var item in value["Commands"])
		{
			data.commands.Add(item.Value);
		}
		foreach(var item in value["Variables"])
		{
			data.variables.Add((KeyValuePair)item.Value);
		}
		data.path = value["Path"];

		return data;
	}


	public StoryObject()
	{
		ID = "";
		type = "Default";
		responses = new List<string>();
		//responses.Add("r");
		//responses.Add("r2");
		destinationTags = new List<string>();
		//destinationTags.Add("d");
		//destinationTags.Add("d2");
		commands = new List<string>();
		//commands.Add("First Command");
		variables = new List<KeyValuePair>();
		//variables.Add(new KeyValuePair("townInvaded", "true"));
		path = "";
	}

	public StoryObject(string i, string p)
	{
		ID = i;
		type = p;
	}

	public void Print()
	{
		Debug.Log(ID);
		Debug.Log(type);
		Debug.Log(path);
		foreach(var item in variables)
		{
			Debug.Log(item);
		}
		foreach(var item in responses)
		{
			Debug.Log(item);
		}
		foreach(var item in destinationTags)
		{
			Debug.Log(item);
		}
		foreach(var item in commands)
		{
			Debug.Log(item);
		}
		foreach(var item in variables)
		{
			item.Print();
		}
	}

	public string ToJsonData()
	{
		JSONObject sObj = (JSONObject) this;
		return sObj.ToString(4);
	}
}
