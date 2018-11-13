using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	public GUISkin skin = null;
	public GUIStyle style;
 	
	// Starting resources
	public int Token;
	public int Health;
	
	public bool win = false;
	public bool lose = false;
 
	public Texture winBackground;
	public Vector2 winBackgroundPos;
	
	public Texture loseBackground;
	public Vector2 loseBackgroundPos;
		
	public Texture levelSelectButtion;
	public Vector2 levelSelectButtionPos;
	
	private SpawnManager spawnManager;
	
    void OnGUI() 
	{
		if(win)
		{
			GUI.Label(new Rect(winBackgroundPos.x, winBackgroundPos.y, winBackground.width, winBackground.height), winBackground, style);
		}
		
		if(lose)
		{
			GUI.Label(new Rect(loseBackgroundPos.x, loseBackgroundPos.y, loseBackground.width, loseBackground.height), loseBackground, style);
		}
		
		if(win || lose)
		{
			if(GUI.Button(new Rect(levelSelectButtionPos.x, levelSelectButtionPos.y, levelSelectButtion.width, levelSelectButtion.height), levelSelectButtion, style))
			{
				Application.LoadLevel("Map Selection");	
			}
			
		}
		
    }
			
	// Use this for initialization
	void Start () 
	{
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Health > 0 && spawnManager.disableSpawning && Enemy.enemyCount <= 0)
		{
			win = true;	
		}
		else if(Health <= 0)
		{
			lose = true;	
		}
	}
}
