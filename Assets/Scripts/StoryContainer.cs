using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class StoryContainer
{
	public List<StoryObject> storyObjects;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public static implicit operator JSONObject(StoryContainer value)
	{
		JSONObject sObject = new JSONObject();

		for(int index = 0; index < value.storyObjects.Count; index++)
		{
			sObject["Story Nodes"][index] = value.storyObjects[index];
		}

		return sObject;
	}


	public static explicit operator StoryContainer(JSONNode value)
	{
		StoryContainer data = new StoryContainer();

		foreach(var item in value["Story Nodes"])
		{
			data.storyObjects.Add((StoryObject)item.Value);
		}

		return data;
	}

	public StoryContainer()
	{
		storyObjects = new List<StoryObject>();
	}

	public string ToJsonData()
	{
		Debug.Log("JSON returning");
		JSONObject sObj = (JSONObject) this;
		return sObj.ToString(4);
	}

	/*
	public void LoadJsonData(string path)
	{
		
		StoryObject obj = new StoryObject();

		if (File.Exists(path))  
		{
			string fileText = File.ReadAllText(path);
			storyObjects = JsonUtility.FromJson<List<StoryObject>>(fileText);
			//dialogueObjectList = JSON.Parse(fileText);
		}
		//obj.Print();
	}
	*/

	public void Print()
	{
		foreach(var item in storyObjects)
		{
			item.Print();
		}
	}
}
