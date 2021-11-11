using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class StoryActionObject : StoryObject
{
	//public List<string> currentDialogueObject;
	//public List<string> nextDialogueObject;
	public List<KeyValuePair> LinesAndDialogueTags;
	//public Dictionary<string, string> testDialogueObject;
	public List<KeyValuePair> Variables;
	string action = "F Test";

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public static implicit operator JSONObject(StoryActionObject value)
	{
		JSONObject sObject = new JSONObject();

		sObject["ID"] = value.ID;
		sObject["Object Type"] = value.type;
		sObject["Action"] = value.action;

		return sObject;
	}


	public static explicit operator StoryActionObject(JSONNode value)
	{
		StoryActionObject data = new StoryActionObject();

		data.ID = value["ID"];
		data.type = value["Object Type"];

		return data;
	}

	public StoryActionObject() : base()
	{
		/*
		currentDialogueObject = new List<string>();
		currentDialogueObject.Add("Test line 1");
		currentDialogueObject.Add("Test line 2");

		nextDialogueObject = new List<string>();
		nextDialogueObject.Add("Answer line 1");
		nextDialogueObject.Add("Answer line 2");
		*/

		//testDialogueObject = new Dictionary<string, string>();
		//string[] s = new string[] {"Test line 1", "q2a"};
		//string s = "test";
		//testDialogueObject.Add("q1a", s);
		LinesAndDialogueTags = new List<KeyValuePair>();
		LinesAndDialogueTags.Add(new KeyValuePair("Test1", "Answer1"));
		LinesAndDialogueTags.Add(new KeyValuePair("Test2", "Answer2"));
	}
}