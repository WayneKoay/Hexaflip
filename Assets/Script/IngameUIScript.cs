using UnityEngine;
using System.Collections;

public class IngameUIScript : MonoBehaviour 
{
	public GUIStyle style;
	
	public Texture topRightBackgroundTexture;
	public Texture helpButtonTexture;
	public Texture pauseButtonTexture;
	public Texture fastForwardButtonTexture;
	public Texture fastForwardButtonTextureActive;
	public Texture fastForwardButtonTextureInactive;
	public Texture waveStartButtonTexture;
	public Texture healthLabelTexture;
	public Texture waveButtonTexture;
	public Texture waveBackgroundTexture;
		
	public Vector2 topRightBackgroundPos;
	public Vector2 helpButtonPos;
	public Vector2 pauseButtonPos;
	public Vector2 fastForwardButtonPos;
	public Vector2 waveStartButtonPos;
	public Vector2 healthLabelPos;
	public Vector2 waveButtonPos;
	public Vector2 waveBackgroundPos;
	
	//Help menu
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
	
	public bool enableHelpMenu;
	public int helpMenuPage = 1;
	
	//Pause menu
	public Texture pauseBackgroundTexture;
	public Texture musicToggleIconTexture;
	public Texture musicToggleTickBoxYesTexture;
	public Texture musicToggleTickBoxNoTexture;
	public Texture mainMenuButtonTexture;
	public Texture resumeButtonTexture;
	
	public Vector2 pauseBackgroundPos;
	public Vector2 musicToggleTickBoxPos;
	public Vector2 mainMenuButtonPos;
	public Vector2 resumeButtonPos;
	
	public bool ingameMute = false;
	
	public bool displayWave = false;
	public bool pause = false;
	public bool doubleSpeed = false;
	public bool endPreparation = false;
	public bool startWave = false;
	
	private MainMenuScript mainMenu;
	private PlayerManager playerManager;
	private SpawnManager spawnManager;
	
	void OnGUI()
	{
		if(!doubleSpeed)
		{
			fastForwardButtonTexture = fastForwardButtonTextureInactive;
		}
		else
		{
			fastForwardButtonTexture = fastForwardButtonTextureActive;
		}
		
		if(!playerManager.win && !playerManager.lose)
		{
			if(!enableHelpMenu)
			{
				if(!pause)
				{
					GUI.Label(new Rect(topRightBackgroundPos.x, topRightBackgroundPos.y, topRightBackgroundTexture.width, topRightBackgroundTexture.height), topRightBackgroundTexture, style);
						
					GUI.Label(new Rect(healthLabelPos.x, healthLabelPos.y, healthLabelTexture.width, healthLabelTexture.height), healthLabelTexture, style);
					GUI.Label(new Rect(healthLabelPos.x - 40.0f, healthLabelPos.y - 5.0f, healthLabelTexture.width, healthLabelTexture.height), playerManager.Health.ToString(), style);
				
					if(!startWave && !spawnManager.disableSpawning && Enemy.enemyCount <= 0)
					{
						if(GUI.Button(new Rect(waveStartButtonPos.x, waveStartButtonPos.y, waveStartButtonTexture.width, waveStartButtonTexture.height), waveStartButtonTexture, style))
						{
							startWave = true;	
							endPreparation = true;
						}
					}
					
					if(GUI.Button(new Rect(helpButtonPos.x, helpButtonPos.y, helpButtonTexture.width, helpButtonTexture.height), helpButtonTexture, style))
					{
						enableHelpMenu = true;	
						pause = TogglePause();
					}
				
					if(GUI.Button(new Rect(pauseButtonPos.x, pauseButtonPos.y, pauseButtonTexture.width, pauseButtonTexture.height), pauseButtonTexture, style))
					{
						pause = TogglePause();	
					}
					
					if(GUI.Button(new Rect(fastForwardButtonPos.x, fastForwardButtonPos.y, fastForwardButtonTexture.width, fastForwardButtonTexture.height), fastForwardButtonTexture, style))
					{
						doubleSpeed = ToggleDoubleSpeed();	
					}
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
					pause = TogglePause();
				}
			}
		}
		
		if(pause && !enableHelpMenu)
		{
			GUI.Label(new Rect(pauseBackgroundPos.x, pauseBackgroundPos.y, pauseBackgroundTexture.width, pauseBackgroundTexture.height), pauseBackgroundTexture, style);
									
			if(!ingameMute)
			{
				if(GUI.Button(new Rect(musicToggleTickBoxPos.x, musicToggleTickBoxPos.y, musicToggleTickBoxYesTexture.width, musicToggleTickBoxYesTexture.height), musicToggleTickBoxYesTexture, style))
				{
					ToggleMute();
				}
			}
			else if(ingameMute)
			{
				if(GUI.Button(new Rect(musicToggleTickBoxPos.x, musicToggleTickBoxPos.y, musicToggleTickBoxNoTexture.width, musicToggleTickBoxNoTexture.height), musicToggleTickBoxNoTexture, style))
				{
					ToggleMute();
				}	
			}
			
			if(GUI.Button(new Rect(mainMenuButtonPos.x, mainMenuButtonPos.y, mainMenuButtonTexture.width, mainMenuButtonTexture.height), mainMenuButtonTexture, style))
			{
				pause = TogglePause();
				Application.LoadLevel("Main Menu");
			}
			if(GUI.Button(new Rect(resumeButtonPos.x, resumeButtonPos.y, resumeButtonTexture.width, resumeButtonTexture.height), resumeButtonTexture, style))
			{
				pause = TogglePause();	
			}
		}
	}
	
	public bool TogglePause()
	{
		if(Time.timeScale == 0.0f)
		{
			Time.timeScale = 1.0f;
			return(false);
		}
		else
		{
			Time.timeScale = 0.0f;
			return(true);
		}
	}
	
	public bool ToggleDoubleSpeed()
	{
		if(Time.timeScale == 2.0f)
		{
			Time.timeScale = 1.0f;
			return(false);
		}
		else
		{
			Time.timeScale = 2.0f;
			return(true);
		}
	}
	
	public void ToggleMute()
	{
		if(GetComponent<AudioSource>().volume > 0)	
		{
			GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt("MuteSound", 1);
			ingameMute = true;
		}
		else
		{
			GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt("MuteSound", 0);
			ingameMute = false;
		}
		PlayerPrefs.Save();
	}
	
	// Use this for initialization
	void Start () 
	{		
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
						
		int muteInt = PlayerPrefs.GetInt("MuteSound");
		if(muteInt == 0)
		{
			ingameMute = false;
			GetComponent<AudioSource>().volume = 1;
		}
		else
		{
			ingameMute = true;
			GetComponent<AudioSource>().volume = 0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
