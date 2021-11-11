using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadResouces : MonoBehaviour
{
	public RectTransform mainMenu;
	public RectTransform storyMenu;

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
		mainMenu.gameObject.SetActive(false);
		storyMenu.gameObject.SetActive(true);
		StoryController.Instance.LoadStory();
	}
}
