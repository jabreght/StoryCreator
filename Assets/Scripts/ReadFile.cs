using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class ReadFile : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	public void onClick ()
	{
		//StoryContainer obj = new StoryContainer();

		if (File.Exists(StoryController.Instance.path))
		{
			string fileText = File.ReadAllText(StoryController.Instance.path);
			JSONNode sObj = JSONObject.Parse(fileText);
			StoryController.Instance.gameStory = (StoryContainer) sObj;
			//GameController.Instance.controller.gameStory.Print();
		}
	}
}