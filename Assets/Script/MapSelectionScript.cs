using UnityEngine;
using System.Collections;

public class MapSelectionScript : MonoBehaviour 
{
	public GUIStyle style;
	
	public Texture mapNumber1Texture;
	public Texture mapNumber2Texture;
	public Texture mapNumber3Texture;
	
	public Vector2 mapNumberPos;
	
	public Texture mapPreview1Texture;
	public Texture mapPreview2Texture;
	public Texture mapPreview3Texture;
	
	public Vector2 mapPreviewPos;
	
	public Texture mapDescription1Texture;
	public Texture mapDescription2Texture;
	public Texture mapDescription3Texture;
	
	public Vector2 mapDescriptionPos;
	
	public Texture leftArrowTexture;
	public Texture rightArrowTexture;
	public Vector2 leftArrowPos;
	public Vector2 rightArrowPos;
	
	public Texture playButtonTexture;
	public Vector2 playButtonPos;
	
	public Texture backButtonTexture;
	public Vector2 backButtonPos;
	
	public int levelSelected = 1;
	
	public bool mapSelectionMute;

	void OnGUI()
	{
		if(levelSelected == 1)
		{
			GUI.Label(new Rect(mapNumberPos.x, mapNumberPos.y, mapNumber1Texture.width, mapNumber1Texture.height), mapNumber1Texture, style);
			GUI.Label(new Rect(mapPreviewPos.x, mapPreviewPos.y, mapPreview1Texture.width, mapPreview1Texture.height), mapPreview1Texture, style);
			GUI.Label(new Rect(mapDescriptionPos.x, mapDescriptionPos.y, mapDescription1Texture.width, mapDescription1Texture.height), mapDescription1Texture, style);
		}
		else if(levelSelected == 2)
		{
			GUI.Label(new Rect(mapNumberPos.x, mapNumberPos.y, mapNumber2Texture.width, mapNumber2Texture.height), mapNumber2Texture, style);
			GUI.Label(new Rect(mapPreviewPos.x, mapPreviewPos.y, mapPreview2Texture.width, mapPreview2Texture.height), mapPreview2Texture, style);
			GUI.Label(new Rect(mapDescriptionPos.x, mapDescriptionPos.y, mapDescription2Texture.width, mapDescription2Texture.height), mapDescription2Texture, style);
		}
		else if(levelSelected == 3)
		{
			GUI.Label(new Rect(mapNumberPos.x, mapNumberPos.y, mapNumber3Texture.width, mapNumber3Texture.height), mapNumber3Texture, style);	
			GUI.Label(new Rect(mapPreviewPos.x, mapPreviewPos.y, mapPreview3Texture.width, mapPreview3Texture.height), mapPreview3Texture, style);
			GUI.Label(new Rect(mapDescriptionPos.x, mapDescriptionPos.y, mapDescription3Texture.width, mapDescription3Texture.height), mapDescription3Texture, style);
		}
		
		if(GUI.Button(new Rect(leftArrowPos.x, leftArrowPos.y, leftArrowTexture.width, leftArrowTexture.height), leftArrowTexture, style))
		{
			if(levelSelected == 1)
			{
				levelSelected = 3;		
			}
			else if(levelSelected == 2)
			{
				levelSelected = 1;		
			}
			else if(levelSelected == 3)
			{
				levelSelected = 2;		
			}
		}
		else if(GUI.Button(new Rect(rightArrowPos.x, rightArrowPos.y, rightArrowTexture.width, rightArrowTexture.height), rightArrowTexture, style))
		{
			if(levelSelected == 1)
			{
				levelSelected = 2;		
			}
			else if(levelSelected == 2)
			{
				levelSelected = 3;		
			}
			else if(levelSelected == 3)
			{
				levelSelected = 1;		
			}
		}
		
		if(GUI.Button(new Rect(playButtonPos.x, playButtonPos.y, playButtonTexture.width, playButtonTexture.height), playButtonTexture, style))
		{
			if(levelSelected == 1)
			{
				Application.LoadLevel("Level 1");
			}
			else if(levelSelected == 2)
			{
				Application.LoadLevel("Level 2");
			}
			else if(levelSelected == 3)
			{
				Application.LoadLevel("Level 3");
			}
		}	
		
		if(GUI.Button(new Rect(backButtonPos.x, backButtonPos.y, backButtonTexture.width, backButtonTexture.height), backButtonTexture, style))
		{
			Application.LoadLevel("Main Menu");
		}
	}
	
	public void ToggleMute()
	{
		if(GetComponent<AudioSource>().volume > 0)	
		{
			GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt("MuteSound", 1);
			mapSelectionMute = true;
		}
		else
		{
			GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt("MuteSound", 0);
			mapSelectionMute = false;
		}
		PlayerPrefs.Save();
	}
	
	// Use this for initialization
	void Start () 
	{
		int muteInt = PlayerPrefs.GetInt("MuteSound");
		if(muteInt == 0)
		{
			mapSelectionMute = false;
			GetComponent<AudioSource>().volume = 1;
		}
		else
		{
			mapSelectionMute = true;
			GetComponent<AudioSource>().volume = 0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
