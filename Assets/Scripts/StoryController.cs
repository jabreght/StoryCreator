using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;

public class StoryController : MonoBehaviour 
{
	private static StoryController instance;

	public static StoryController Instance
	{
		get
		{
			return instance ?? (instance = new GameObject("Story Controller").AddComponent<StoryController>());
		}
	}
	
	public enum Conditionals {Equals, NotEquals, GreaterThan, LessThan};

	public string path = "Assets/Resources/Dialogue/textwrite.json";

	//Variables for button creation
	public GameObject currentBackgroundObject;
	public GameObject videoPlayerObject;
	public GameObject questionsPanelObject;
	public UnityEngine.UI.Image currentBackground;
	public UnityEngine.Video.VideoPlayer videoPlayer;
	public RectTransform questionsPanel;
	public GameObject buttonPrefab;
	protected List<GameObject> buttonList;
	private Dictionary<string,Sprite> gameGFX;
	private Dictionary<string,AudioClip> gameSFX;

	public int test = 4;

	//public List<DialogueObject> dialogueObjectList;
	public StoryContainer gameStory;
	public Dictionary<string, int> storyObjectIndex;
	public Dictionary<string, string> storyVariables;

	public string currentNodeID;
	public bool loadNewNode;
	public StoryObject currentNode;


	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(loadNewNode)
		{
			Debug.Log("loading new node : " + currentNodeID);
			loadNewNode = false;
			currentNode = gameStory.storyObjects[storyObjectIndex[currentNodeID]];
			StartCoroutine(ReadNodeType(currentNode));
		}
	}


	public StoryController()
	{
		gameStory = new StoryContainer();
		storyObjectIndex = new Dictionary<string, int>();
		storyVariables = new Dictionary<string, string>();
		gameGFX = new Dictionary<string, Sprite>();
		gameSFX = new Dictionary<string, AudioClip>();

		buttonList = new List<GameObject>();
	}


	public void LoadStory(string ID = "1")
	{
		LoadJsonData(path);
		SetupDictionaryIndex();
		LoadResources();
		ChangeCurrentNodeID(ID);
		Debug.Log("start");
	}

	private void SetupDictionaryIndex()
	{
		int index = 0;
		foreach(var item in gameStory.storyObjects)
		{
			try
			{
				storyObjectIndex.Add(item.ID, index);
				index++;
			}
			catch
			{
				Debug.Log("ID already exists in Dictionary - " + item.ID);
			}
		}
	}


	private void ChangeCurrentNodeID(string ID)
	{
		currentNodeID = ID;
		loadNewNode = true;
	}


	private IEnumerator ReadNodeType(StoryObject  currentStoryObject)
	{
		DisableButtons();
		switch(currentStoryObject.type.ToUpper())
		{
			case "DIALOGUE":
				print("Dialogue");
				SetupActiveButtons(currentNode.responses.Count);
				SetupDialogue(currentNode);
				break;
			case "SET VARIABLE":
				SetVariables(currentNode);
				break;
			case "CHANGE VARIABLE":
				ChangeVariables(currentNode);
				break;
			case "CHANGE BACKGROUND":
				ChangeBackground(currentNode);
				break;
			case "CONDITIONAL EQUALS":
				if (EvaluateConditional(currentNode, Conditionals.Equals))
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
				}
				else
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[1]);
				}
				break;
			case "CONDITIONAL NOT EQUALS":
				if (EvaluateConditional(currentNode, Conditionals.NotEquals))
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
				}
				else
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[1]);
				}
				break;
			case "CONDITIONAL GREATER THAN":
				if (EvaluateConditional(currentNode, Conditionals.GreaterThan))
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
				}
				else
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[1]);
				}
				break;
			case "CONDITIONAL LESS THAN":
				if (EvaluateConditional(currentNode, Conditionals.LessThan))
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
				}
				else
				{
					ChangeCurrentNodeID(currentStoryObject.destinationTags[1]);
				}
				break;
			default:
				//Possibly save players JSON back to a file printing Error variable with error message
				Debug.Log("Invalid Type");
				break;
		}
		yield return null;
	}


	private void SetVariables(StoryObject  currentStoryObject)
	{
		foreach(var item in currentStoryObject.variables)
		{
			storyVariables[item.key] = item.value;
		}

		ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
	}


	private void ChangeVariables(StoryObject  currentStoryObject)
	{
		foreach(var item in currentStoryObject.variables)
		{
			switch(item.key[0].ToString().ToUpper())
			{
				case "I":
					storyVariables[item.key] = (int.Parse(storyVariables[item.key]) + int.Parse(item.value)).ToString();
					break;
				case "F":
					storyVariables[item.key] = (float.Parse(storyVariables[item.key]) + float.Parse(item.value)).ToString();
					break;
				case "S":
				case "B":
				default:
					storyVariables[item.key] = item.value;
					break;

			}
		}

		ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
	}


	private void ChangeBackground(StoryObject currentStoryObject)
	{
		currentBackground.sprite = gameGFX[currentStoryObject.path];
		currentBackground.color = Color.white;

		ChangeCurrentNodeID(currentStoryObject.destinationTags[0]);
	}


	public bool EvaluateConditional(StoryObject currentStoryObject, Conditionals op)
	{
		if (currentStoryObject.variables[0].key[0].ToString().ToUpper().Equals("I"))
		{
			switch(op)
			{
				case Conditionals.Equals:
					return (int.Parse(currentStoryObject.variables[0].value) == int.Parse(storyVariables[currentStoryObject.variables[0].key]) );
				case Conditionals.NotEquals:
					return !(int.Parse(currentStoryObject.variables[0].value) == int.Parse(storyVariables[currentStoryObject.variables[0].key]) );
				case Conditionals.GreaterThan:
					return (int.Parse(currentStoryObject.variables[0].value) < int.Parse(storyVariables[currentStoryObject.variables[0].key]) );
				case Conditionals.LessThan:
					return (int.Parse(currentStoryObject.variables[0].value) > int.Parse(storyVariables[currentStoryObject.variables[0].key]) );
			}
		}
		else if (currentStoryObject.variables[0].key[0].ToString().ToUpper().Equals("F"))
		{
			switch(op)
			{
				case Conditionals.Equals:
					return Mathf.Approximately( float.Parse(currentStoryObject.variables[0].value), float.Parse(storyVariables[currentStoryObject.variables[0].key]) );
				case Conditionals.NotEquals:
					return !Mathf.Approximately( float.Parse(currentStoryObject.variables[0].value), float.Parse(storyVariables[currentStoryObject.variables[0].key]) );
				case Conditionals.GreaterThan:
					return float.Parse(currentStoryObject.variables[0].value) < float.Parse(storyVariables[currentStoryObject.variables[0].key]);
				case Conditionals.LessThan:
					return float.Parse(currentStoryObject.variables[0].value) > float.Parse(storyVariables[currentStoryObject.variables[0].key]);
			}
		}
		return false;
	}

	/*
	public void EvaluateConditional(StoryObject currentStoryObject, bool value)
	{
		
	}
	/*

	/*
	private T ReturnVariable<T>(StoryObject currentStoryObject)
	{
		T t = typeof(T);
	}
	*/

	private void SetupDialogue(StoryObject  currentStoryObject)
	{
		for(int index = 0; index < currentStoryObject.responses.Count; index++)
		{
			buttonList[index].GetComponentInChildren<UnityEngine.UI.Text>().text = currentStoryObject.responses[index];
		}
	}


	//Set needed buttons to be active, or add more if more are needed
	private void SetupActiveButtons(int count)
	{
		for(int index = 0; index < count; index++)
		{
			int capturedIndex = index;
			if (buttonList.Count - 1 < index)
			{	
				buttonList.Add((GameObject)Instantiate(buttonPrefab));
				buttonList[index].name = "btQ" + index;
				buttonList[index].transform.SetParent(questionsPanel);
				buttonList[index].transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
				buttonList[index].transform.position = new Vector3(0f, 0f ,0f);
			}
			else
			{
				buttonList[index].SetActive(true);
			}

			buttonList[index].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => DialogueButtonOnClick(capturedIndex));
		}
	}


	private void DialogueButtonOnClick(int index)
	{
		ChangeCurrentNodeID(currentNode.destinationTags[index]);
	}


	//Load JSON data from given path
	public void LoadJsonData(string path)
	{
		if (File.Exists(path))
		{
			string fileText = File.ReadAllText(path);
			//TextAsset p = Resources.Load<TextAsset>(path);
			JSONNode sObj = JSONObject.Parse(fileText);
			gameStory = (StoryContainer) sObj;
		}
		else
		{
			Debug.Log("Path doesn't exist");
		}
	}


	//Load prefabs so buttons can be instantiated
	public void LoadResources()
	{
		//Set up scene objects and prefabs

		currentBackground = GameObject.Find("Background").GetComponent<UnityEngine.UI.Image>();
		questionsPanel = GameObject.Find("Questions Panel").GetComponent<RectTransform>();
		buttonPrefab = Resources.Load("Prefabs/btnQuestionPrefab") as GameObject;
		//Load required SFX and GFX resources from external sources

		if (storyObjectIndex.ContainsKey("GFX Resources"))
		{
			string localPath = gameStory.storyObjects[storyObjectIndex["GFX Resources"]].path;
			if (Directory.Exists(localPath))
			{
				foreach (var item in Directory.GetFiles(localPath))
				{
					Texture2D tex = new Texture2D(2,2);
					tex.LoadImage(File.ReadAllBytes(item));
					gameGFX.Add(Path.GetFileNameWithoutExtension(item), Sprite.Create(tex, new Rect(0,0,tex.width,tex.height),new Vector2(0,0)));
				}
			}
		}
		if (storyObjectIndex.ContainsKey("SFX Resources"))
		{
			string localPath = gameStory.storyObjects[storyObjectIndex["SFX Resources"]].path;
			if (Directory.Exists(localPath))
			{
				foreach (var item in Directory.GetFiles(localPath))
				{
					
					gameSFX.Add(Path.GetFileNameWithoutExtension(item), new AudioClip());
				}
			}
		}
		//backgroundGFX = new List<Sprite>(Resources.LoadAll<Sprite>(""));
	}


	//Set buttons to not be active for next scene
	public void DisableButtons()
	{
		foreach(GameObject obj in buttonList)
		{
			obj.SetActive(false);
		}
	}
}