using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	// Singleton
	/*
	private static GameController instance;

	public static GameController Instance
	{
		get
		{
			return instance ?? (instance = new GameObject("Game Controller").AddComponent<GameController>());
		}
	}
	*/
	public StoryController controller;
	public string path = "Assets/Resources/Dialogue/textwrite.json";

	protected GameController()
	{
		//controller = new StoryController();
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		print("single");
	}

	void Awake()
	{
		if (controller == null)
		{
			controller = new StoryController();
		}
	}
}
