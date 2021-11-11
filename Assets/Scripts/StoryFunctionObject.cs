using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class StoryFunctionObject : StoryObject
{
	string function = "F Test";

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public static implicit operator JSONObject(StoryFunctionObject value)
	{
		JSONObject sObject = new JSONObject();

		sObject["ID"] = value.ID;
		sObject["Object Type"] = value.type;
		sObject["Function"] = value.function;

		return sObject;
	}


	public static explicit operator StoryFunctionObject(JSONNode value)
	{
		StoryFunctionObject data = new StoryFunctionObject();

		data.ID = value["ID"];
		data.type = value["Object Type"];

		return data;
	}

	public StoryFunctionObject() : base()
	{
	}
}