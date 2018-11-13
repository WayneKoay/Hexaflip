using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour 
{
	public GUIStyle style;
	
	public Texture background;
	public Texture gameLogo;
	public Texture startButton;
	public Texture soundOnButton;
	public Texture soundOffButton;
	public Texture helpButton;
	public Texture creditButton;
	
	public Vector2 backgroundPos;
	public Vector2 gameLogoPos;
	public Vector2 startButtonPos;
	public Vector2 soundOnButtonPos;
	public Vector2 soundOffButtonPos;
	public Vector2 helpButtonPos;
	public Vector2 creditButtonPos;
	
	public Texture helpGameScreenExplanation;
	public Texture helpTileAndTowerBehaviour;
	public Texture helpSingleTowers;
	public Texture helpDoubleTowers;
	public Texture helpComboTowersTower;
	public Texture helpPrevButton;
	public Texture helpNextButton;
	public Texture helpCloseButton;
	
	public Vector2 helpGameScreenExplanationPos;
	public Vector2 helpTileAndTowerBehaviourPos;
	public Vector2 helpSingleTowersPos;
	public Vector2 helpDoubleTowersPos;
	public Vector2 helpComboTowersTowerPos;
	public Vector2 helpPrevButtonPos;
	public Vector2 helpNextButtonPos;
	public Vector2 helpCloseButtonPos;
	
	public Texture credit;
	public Vector2 creditPos;
	
	public bool mainMenumute = false;
	public bool enableHelpMenu = false;
	public bool enableCredit = false;
	public int helpMenuPage = 1;
	
	void OnGUI()
	{
		if(!enableHelpMenu)
		{
			GUI.Label(new Rect(backgroundPos.x, backgroundPos.y, background.width, background.height), background, style);
			GUI.Label(new Rect(gameLogoPos.x, gameLogoPos.y, gameLogo.width, gameLogo.height), gameLogo, style);
			
			if(GUI.Button(new Rect(startButtonPos.x, startButtonPos.y, startButton.width, startButton.height), startButton, style))
			{
				Application.LoadLevel("Map Selection");	
			}
			
			if(!mainMenumute)
			{
				if(GUI.Button(new Rect(soundOnButtonPos.x, soundOnButtonPos.y, soundOnButton.width, soundOnButton.height), soundOnButton, style))
				{
					ToggleMute();
				}
			}
			else if(mainMenumute)
			{
				if(GUI.Button(new Rect(soundOffButtonPos.x, soundOffButtonPos.y, soundOffButton.width, soundOffButton.height), soundOffButton, style))
				{
					ToggleMute();
				}	
			}
			
			if(GUI.Button(new Rect(helpButtonPos.x, helpButtonPos.y, helpButton.width, helpButton.height), helpButton, style))
			{
				enableHelpMenu = true;
			}
			
			if(GUI.Button(new Rect(creditButtonPos.x, creditButtonPos.y, creditButton.width, creditButton.height), creditButton, style))
			{
				enableCredit = true;
			}
		}
		
		else if(enableHelpMenu)
		{
			if(helpMenuPage == 1)
			{
				GUI.Label(new Rect(helpGameScreenExplanationPos.x, helpGameScreenExplanationPos.y, helpGameScreenExplanation.width, helpGameScreenExplanation.height), helpGameScreenExplanation, style);
			}
			else if(helpMenuPage == 2)
			{
				GUI.Label(new Rect(helpTileAndTowerBehaviourPos.x, helpTileAndTowerBehaviourPos.y, helpTileAndTowerBehaviour.width, helpTileAndTowerBehaviour.height), helpTileAndTowerBehaviour, style);
			}
			else if(helpMenuPage == 3)
			{
				GUI.Label(new Rect(helpSingleTowersPos.x, helpSingleTowersPos.y, helpSingleTowers.width, helpSingleTowers.height), helpSingleTowers, style);
			}
			else if(helpMenuPage == 4)
			{
				GUI.Label(new Rect(helpDoubleTowersPos.x, helpDoubleTowersPos.y, helpDoubleTowers.width, helpDoubleTowers.height), helpDoubleTowers, style);
			}
			else if(helpMenuPage == 5)
			{
				GUI.Label(new Rect(helpComboTowersTowerPos.x, helpComboTowersTowerPos.y, helpComboTowersTower.width, helpComboTowersTower.height), helpComboTowersTower, style);
			}
			
			if(helpMenuPage != 1)
			{
				if(GUI.Button(new Rect(helpPrevButtonPos.x, helpPrevButtonPos.y, helpPrevButton.width, helpPrevButton.height), helpPrevButton, style))
				{
					if(helpMenuPage == 2)
					{
						helpMenuPage = 1;	
					}
					else if(helpMenuPage == 3)
					{
						helpMenuPage = 2;	
					}
					else if(helpMenuPage == 4)
					{
						helpMenuPage = 3;	
					}
					else if(helpMenuPage == 5)
					{
						helpMenuPage = 4;	
					}
				}
			}
			if(helpMenuPage != 5)
			{
				if(GUI.Button(new Rect(helpNextButtonPos.x, helpNextButtonPos.y, helpNextButton.width, helpNextButton.height), helpNextButton, style))
				{
					if(helpMenuPage == 1)
					{
						helpMenuPage = 2;	
					}
					else if(helpMenuPage == 2)
					{
						helpMenuPage = 3;	
					}
					else if(helpMenuPage == 3)
					{
						helpMenuPage = 4;	
					}
					else if(helpMenuPage == 4)
					{
						helpMenuPage = 5;	
					}
				}
			}
			
			if(GUI.Button(new Rect(helpCloseButtonPos.x, helpCloseButtonPos.y, helpCloseButton.width, helpCloseButton.height), helpCloseButton, style))
			{
				enableHelpMenu = false;
			}
		}
		
		if(enableCredit)
		{
			if(GUI.Button(new Rect(creditPos.x, creditPos.y, credit.width, credit.height), credit, style))
			{
				enableCredit = false;	
			}
		}
		
	}
	
	public void ToggleMute()
	{
		if(GetComponent<AudioSource>().volume > 0)	
		{
			GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt("MuteSound", 1);
			mainMenumute = true;
		}
		else
		{
			GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt("MuteSound", 0);
			mainMenumute = false;
		}
		PlayerPrefs.Save();
	}
	
	// Use this for initialization
	void Start () 
	{		
		int muteInt = PlayerPrefs.GetInt("MuteSound");
		if(muteInt == 0)
		{
			mainMenumute = false;
			GetComponent<AudioSource>().volume = 1;
		}
		else
		{
			mainMenumute = true;
			GetComponent<AudioSource>().volume = 0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
