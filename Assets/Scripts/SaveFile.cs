using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class SaveFile : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void onClick()
	{
		StoryContainer container = new StoryContainer();
		StreamWriter writer = new StreamWriter(StoryController.Instance.path);

		writer.Write(container.ToJsonData());
		writer.Close();
	}
}
