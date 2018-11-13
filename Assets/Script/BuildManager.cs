using UnityEngine;
using System.Collections;

public class BuildManager : MonoBehaviour 
{
	public GUIStyle style;
	public GUIStyle style2;
	public bool enableBuildDotTower;
	public bool enableBuildAoeTower;
	public bool enableBuildDisTower;
		
	public bool enableMoveTower;
	
	public bool enableFlipLavaTile;
	public bool enableFlipSnowTile;
	public bool enableFlipScifiTile;
	public bool enableFlipNormalTile;
	
    // Tower prefab
    public GameObject NormalDotTowerPrefab = null;
	public GameObject NormalAoeTowerPrefab = null;
	public GameObject NormalDisTowerPrefab = null;
	
    public GameObject DoubleDotTowerPrefab = null;
	public GameObject DoubleAoeTowerPrefab = null;
	public GameObject DoubleDisTowerPrefab = null;
	
	public GameObject DisAoeTowerPrefab = null;
	public GameObject DisDotTowerPrefab = null;
    public GameObject DotAoeTowerPrefab = null;

	private TouchControl touchControl;
	private PlayerManager playerManager;
	private TileManager tileManager;
	private GridManager gridManager;
	
	public Texture BuildTowerTexture;
	public Texture BuildTowerTextureInactive;
	public Texture BuildTowerTextureActive;
	
	public Texture BuildAoeTowerTexture;
	public Texture BuildAoeTowerTextureInactive;
	public Texture BuildAoeTowerTextureActive;
	
	public Texture BuildDisTowerTexture;
	public Texture BuildDisTowerTextureInactive;
	public Texture BuildDisTowerTextureActive;
	
	public Texture BuildDotTowerTexture;
	public Texture BuildDotTowerTextureInactive;
	public Texture BuildDotTowerTextureActive;
	
	public Texture SellTowerTexture;
	public Texture SellTowerTextureInactive;
	public Texture SellTowerTextureActive;
	
	public Texture BuildTileTexture;
	public Texture BuildTileTextureInactive;
	public Texture BuildTileTextureActive;
	
	public Texture BuildAoeTileTexture;
	public Texture BuildAoeTileTextureInactive;
	public Texture BuildAoeTileTextureActive;
	
	public Texture BuildDisTileTexture;
	public Texture BuildDisTileTextureInactive;
	public Texture BuildDisTileTextureActive;
	
	public Texture BuildDotTileTexture;
	public Texture BuildDotTileTextureInactive;
	public Texture BuildDotTileTextureActive;
	
	public Texture TokenTexture;
	public Vector2 TokenPos;
	
	public Vector2 BuildTowerPos;
	public Vector2 BuildAoeTowerPos;
	public Vector2 BuildDisTowerPos;
	public Vector2 BuildDotTowerPos;
	public Vector2 SellTowerPos;
	
	public Vector2 BuildTilePos;
	public Vector2 BuildAoeTilePos;
	public Vector2 BuildDisTilePos;
	public Vector2 BuildDotTilePos;
	
	public bool overGUIElement = false;
	
	private IngameUIScript ingameUI;
	
	// Use this for initialization
	void Start () 
	{
		touchControl = GameObject.Find("PlayerManager").GetComponent<TouchControl>();
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		ingameUI = GameObject.Find("IngameUI").GetComponent<IngameUIScript>();
	}	
 	
    void OnGUI() 
	{    		
		//Change tile buttons texture if tile buttons are clicked
		if(!enableFlipLavaTile)
		{
			BuildAoeTileTexture = BuildAoeTileTextureInactive;
		}
		else
		{
			BuildAoeTileTexture = BuildAoeTileTextureActive;
		}
		if(!enableFlipScifiTile)
		{
			BuildDisTileTexture = BuildDisTileTextureInactive;
		}
		else
		{
			BuildDisTileTexture = BuildDisTileTextureActive;
		}
		if(!enableFlipSnowTile)
		{
			BuildDotTileTexture = BuildDotTileTextureInactive;
		}
		else
		{
			BuildDotTileTexture = BuildDotTileTextureActive;
		}
		
		//Change tower buttons texture if tower buttons are clicked
		if(!enableBuildAoeTower)
		{
			BuildAoeTowerTexture = BuildAoeTowerTextureInactive;
		}
		else
		{
			BuildAoeTowerTexture = BuildAoeTowerTextureActive;
		}
		if(!enableBuildDisTower)
		{
			BuildDisTowerTexture = BuildDisTowerTextureInactive;
		}
		else
		{
			BuildDisTowerTexture = BuildDisTowerTextureActive;
		}
		if(!enableBuildDotTower)
		{
			BuildDotTowerTexture = BuildDotTowerTextureInactive;
		}
		else
		{
			BuildDotTowerTexture = BuildDotTowerTextureActive;
		}
		if(!enableMoveTower)
		{
			SellTowerTexture = SellTowerTextureInactive;
		}
		else
		{
			SellTowerTexture = SellTowerTextureActive;
		}
		
		if(!playerManager.win && !playerManager.lose && !ingameUI.pause)
		{
			if(!ingameUI.enableHelpMenu)
			{
				GUI.Label(new Rect(TokenPos.x, TokenPos.y, TokenTexture.width, TokenTexture.height), TokenTexture, style);
				GUI.Label(new Rect(TokenPos.x - 100.0f, TokenPos.y - 10.0f, TokenTexture.width, TokenTexture.height), playerManager.Token.ToString(), style);
				
				if(!ingameUI.startWave && Enemy.enemyCount <= 0)
				{
					GUI.Button(new Rect(BuildTowerPos.x, BuildTowerPos.y, BuildTowerTexture.width, BuildTowerTexture.height), BuildTowerTexture, style);
					
					Rect BuildAoeTowerButton;
					if(GUI.Button(BuildAoeTowerButton = new Rect(BuildAoeTowerPos.x, BuildAoeTowerPos.y, BuildAoeTowerTexture.width, BuildAoeTowerTexture.height), BuildAoeTowerTexture, style))
					{			
						if(!enableBuildAoeTower && playerManager.Token > 0)
						{
							enableBuildDotTower = false;
							enableBuildAoeTower = true;
							enableBuildDisTower = false;
							
							enableMoveTower = false;
							
							enableFlipLavaTile = false;
							enableFlipSnowTile = false;
							enableFlipScifiTile = false;
						}
						else
						{
							enableBuildAoeTower = false;
						}	
					}
					
					Rect BuildDisTowerButton;
					if(GUI.Button(BuildDisTowerButton = new Rect(BuildDisTowerPos.x, BuildDisTowerPos.y, BuildDisTowerTexture.width, BuildDisTowerTexture.height), BuildDisTowerTexture, style))
					{			
						if(!enableBuildDisTower && playerManager.Token > 0)
						{
							enableBuildDotTower = false;
							enableBuildAoeTower = false;
							enableBuildDisTower = true;
							
							enableMoveTower = false;
							
							enableFlipLavaTile = false;
							enableFlipSnowTile = false;
							enableFlipScifiTile = false;
						}
						else
						{
							enableBuildDisTower = false;
						}	
					}
					
					Rect BuildDotTowerButton;
					if(GUI.Button(BuildDotTowerButton = new Rect(BuildDotTowerPos.x, BuildDotTowerPos.y, BuildDotTowerTexture.width, BuildDotTowerTexture.height), BuildDotTowerTexture, style))
					{			
						if(!enableBuildDotTower && playerManager.Token > 0)
						{
							enableBuildDotTower = true;
							enableBuildAoeTower = false;
							enableBuildDisTower = false;
							
							enableMoveTower = false;
							
							enableFlipLavaTile = false;
							enableFlipSnowTile = false;
							enableFlipScifiTile = false;
						}
						else
						{
							enableBuildDotTower = false;
						}
					}
					
					Rect SellTowerButton;
					if(GUI.Button(SellTowerButton = new Rect(SellTowerPos.x, SellTowerPos.y, SellTowerTexture.width, SellTowerTexture.height), SellTowerTexture, style))
					{
						if(!enableMoveTower)
						{
							enableFlipLavaTile = false;
							enableFlipSnowTile = false;
							enableFlipScifiTile = false;
							
							enableMoveTower = true;
							
							enableBuildDotTower = false;
							enableBuildAoeTower = false;
							enableBuildDisTower = false;
						}
						else
						{
							enableMoveTower = false;
						}
					}
					
					if(enableBuildDotTower || enableBuildAoeTower || enableBuildDisTower)
					{
						Rect alertButton = new Rect(0, 100, 150, 40);
			    		GUI.Box(alertButton, "Select a grid \n to build a tower.", style2);
						
						touchControl.selectedGameObject = null;
					}
					
					//===================================================================================
					// Move Tower button
					if(enableMoveTower)
					{
						Rect alertButton = new Rect(0, 100, 150, 40);
			    		GUI.Box(alertButton, "Select a tower \n to sell.", style2);
						
						touchControl.selectedGameObject = null;
					}
				
					Event e = Event.current;
					if( BuildAoeTowerButton.Contains(e.mousePosition) ||
						BuildDisTowerButton.Contains(e.mousePosition) ||
						BuildDotTowerButton.Contains(e.mousePosition) ||
						SellTowerButton.Contains(e.mousePosition))
					{
						overGUIElement = true;
					}
					else if(!BuildAoeTowerButton.Contains(e.mousePosition) &&
							!BuildDisTowerButton.Contains(e.mousePosition) &&
							!BuildDotTowerButton.Contains(e.mousePosition) &&
							!SellTowerButton.Contains(e.mousePosition))
					{
						overGUIElement = false;	
					}
				}
				//===================================================================================
				// Lava Tile button
				
				GUI.Button(new Rect(BuildTilePos.x, BuildTilePos.y, BuildTileTexture.width, BuildTileTexture.height), BuildTileTexture, style);
				
				Rect BuildAoeTileButton;
				if(GUI.Button(BuildAoeTileButton = new Rect(BuildAoeTilePos.x, BuildAoeTilePos.y, BuildAoeTileTexture.width, BuildAoeTileTexture.height), BuildAoeTileTexture, style))
				{
					if(!enableFlipLavaTile && playerManager.Token > 0)
					{
						enableFlipLavaTile = true;
						enableFlipSnowTile = false;
						enableFlipScifiTile = false;
						
						enableMoveTower = false;
						
						enableBuildDotTower = false;
						enableBuildAoeTower = false;
						enableBuildDisTower = false;
					}
					else
					{
						enableFlipLavaTile = false;
					}
				}
				
				// Snow Tile button
		       	Rect BuildDotTileButton;
				if(GUI.Button(BuildDotTileButton = new Rect(BuildDotTilePos.x, BuildDotTilePos.y, BuildDotTileTexture.width, BuildDotTileTexture.height), BuildDotTileTexture, style))
				{
					if(!enableFlipSnowTile && playerManager.Token > 0)
					{
						enableFlipLavaTile = false;
						enableFlipSnowTile = true;
						enableFlipScifiTile = false;
						
						enableMoveTower = false;
						
						enableBuildDotTower = false;
						enableBuildAoeTower = false;
						enableBuildDisTower = false;
					}
					else
					{
						enableFlipSnowTile = false;
					}
				}
				
				// Scifi Tile button
		       	Rect BuildDisTileButton;
				if(GUI.Button(BuildDisTileButton = new Rect(BuildDisTilePos.x, BuildDisTilePos.y, BuildDisTileTexture.width, BuildDisTileTexture.height), BuildDisTileTexture, style))
				{
					if(!enableFlipScifiTile && playerManager.Token > 0)
					{
						enableFlipLavaTile = false;
						enableFlipSnowTile = false;
						enableFlipScifiTile = true;
						
						enableMoveTower = false;
						
						enableBuildDotTower = false;
						enableBuildAoeTower = false;
						enableBuildDisTower = false;
					}
					else
					{
						enableFlipScifiTile = false;
					}
				}
			
				if(enableFlipLavaTile || enableFlipSnowTile || enableFlipScifiTile/* || enableFlipNormalTile*/)
				{
					Rect alertButton2 = new Rect(0, 200, 150, 40);
		    		GUI.Box(alertButton2, "Select a grid \n with tower \nto flip a tile.", style2);
						
						touchControl.selectedGameObject = null;
				}
					
				Event e2 = Event.current;
				if( BuildAoeTileButton.Contains(e2.mousePosition) ||
					BuildDotTileButton.Contains(e2.mousePosition) ||
					BuildDisTileButton.Contains(e2.mousePosition))
				{
					overGUIElement = true;
				}
				else if(!BuildAoeTileButton.Contains(e2.mousePosition) &&
						!BuildDotTileButton.Contains(e2.mousePosition) &&
						!BuildDisTileButton.Contains(e2.mousePosition))
				{
					overGUIElement = false;
				}
			}
		}
    }
 
    public void Update() 
	{
		if(enableBuildDotTower || enableBuildAoeTower || enableBuildDisTower)
		{	
			if(touchControl.selectedGameObject != null)
			{
				CheckToBuildTower(touchControl.selectedGameObject);
			}
		}
		if(enableFlipLavaTile || enableFlipSnowTile || enableFlipScifiTile/* || enableFlipNormalTile*/)
		{
			if(touchControl.selectedGameObject != null)
			{
				CheckToFlipTile(touchControl.selectedGameObject);
			}
		}
		if(enableMoveTower)
		{
			if(touchControl.selectedGameObject != null)
			{
				CheckToMoveTower(touchControl.selectedGameObject);
			}
		}
    }
	
	void CheckToBuildTower(GameObject obj)
	{
		if(obj.tag == "HexGrid")
		{
			Tile objTile = obj.GetComponent<Tile>();
			
			if(objTile.mCanBeBuiltOn)
			{
				GameObject tempTower = null;
				Transform objTrans = obj.transform;
				
				Vector3 buildPos = objTrans.position + new Vector3(0.0f, 300.0f, 0.0f);
				
				if(enableBuildDotTower && gridManager.CheckPathExist(objTrans.position) && !objTile.mIsToBeFlipped)
				{
					if(objTile.mTileType == Tile.Type.NORMAL_GRASS)
					{
						tempTower = ObjectPool.Instance.instantiate(NormalDotTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.AOE_LAVA)
					{
						tempTower = ObjectPool.Instance.instantiate(DotAoeTowerPrefab, buildPos, Quaternion.identity);
						
					}
					else if(objTile.mTileType == Tile.Type.DOT_SNOW)
					{
						tempTower = ObjectPool.Instance.instantiate(DoubleDotTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.DIS_SCIFI)
					{
						tempTower = ObjectPool.Instance.instantiate(DisDotTowerPrefab, buildPos, Quaternion.identity);
					}		
					playerManager.Token -= 1;
					enableBuildDotTower = false;	
					
					tempTower.GetComponent<Tower>().parentTransform = objTrans;
					
					return;
				}
				else if(enableBuildAoeTower && gridManager.CheckPathExist(objTrans.position) && !objTile.mIsToBeFlipped)
				{
					if(objTile.mTileType == Tile.Type.NORMAL_GRASS)
					{
						tempTower = ObjectPool.Instance.instantiate(NormalAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.AOE_LAVA)
					{
						tempTower = ObjectPool.Instance.instantiate(DoubleAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.DOT_SNOW)
					{
						tempTower = ObjectPool.Instance.instantiate(DotAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					
					else if(objTile.mTileType == Tile.Type.DIS_SCIFI)
					{
						tempTower = ObjectPool.Instance.instantiate(DisAoeTowerPrefab, buildPos, Quaternion.identity);
					}			
					playerManager.Token -= 1;
					enableBuildAoeTower = false;
					
					tempTower.GetComponent<Tower>().parentTransform = objTrans;
					
					return;
				}
				else if(enableBuildDisTower && gridManager.CheckPathExist(objTrans.position) && !objTile.mIsToBeFlipped)
				{					
					if(objTile.mTileType == Tile.Type.NORMAL_GRASS)
					{
						tempTower = ObjectPool.Instance.instantiate(NormalDisTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.AOE_LAVA)
					{
						tempTower = ObjectPool.Instance.instantiate(DisAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.DOT_SNOW)
					{
						tempTower = ObjectPool.Instance.instantiate(DisDotTowerPrefab, buildPos, Quaternion.identity);
					}
					
					else if(objTile.mTileType == Tile.Type.DIS_SCIFI)
					{
						tempTower = ObjectPool.Instance.instantiate(DoubleDisTowerPrefab, buildPos, Quaternion.identity);
					}			
					playerManager.Token -= 1;
					enableBuildDisTower = false;
					
					tempTower.GetComponent<Tower>().parentTransform = objTrans;
					
					return;
				}
			}
		}
	}
	
	void AutoBuildTower(GameObject obj, int towerTypeToBuild)
	{
		if(obj.tag == "HexGrid")
		{
			Tile objTile = obj.GetComponent<Tile>();
			
			if(objTile.mCanBeBuiltOn)
			{
				GameObject tempTower = null;
				Transform objTrans = obj.transform;
				
				Vector3 buildPos = objTrans.position + new Vector3(0.0f, 300.0f, 0.0f);
				
				if(towerTypeToBuild == 1 && gridManager.CheckPathExist(objTrans.position) && !objTile.mIsToBeFlipped)
				{
					if(objTile.mTileType == Tile.Type.NORMAL_GRASS)
					{
						tempTower = ObjectPool.Instance.instantiate(NormalDotTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.AOE_LAVA)
					{
						tempTower = ObjectPool.Instance.instantiate(DotAoeTowerPrefab, buildPos, Quaternion.identity);
						
					}
					else if(objTile.mTileType == Tile.Type.DOT_SNOW)
					{
						tempTower = ObjectPool.Instance.instantiate(DoubleDotTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.DIS_SCIFI)
					{
						tempTower = ObjectPool.Instance.instantiate(DisDotTowerPrefab, buildPos, Quaternion.identity);
					}			
					
					tempTower.GetComponent<Tower>().parentTransform = objTrans;
					
					return;
				}
				else if(towerTypeToBuild == 2 && gridManager.CheckPathExist(objTrans.position) && !objTile.mIsToBeFlipped)
				{
					if(objTile.mTileType == Tile.Type.NORMAL_GRASS)
					{
						tempTower = ObjectPool.Instance.instantiate(NormalAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.AOE_LAVA)
					{
						tempTower = ObjectPool.Instance.instantiate(DoubleAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.DOT_SNOW)
					{
						tempTower = ObjectPool.Instance.instantiate(DotAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					
					else if(objTile.mTileType == Tile.Type.DIS_SCIFI)
					{
						tempTower = ObjectPool.Instance.instantiate(DisAoeTowerPrefab, buildPos, Quaternion.identity);
					}		
					
					tempTower.GetComponent<Tower>().parentTransform = objTrans;
					
					return;
				}
				else if(towerTypeToBuild == 3 && gridManager.CheckPathExist(objTrans.position) && !objTile.mIsToBeFlipped)
				{					
					if(objTile.mTileType == Tile.Type.NORMAL_GRASS)
					{
						tempTower = ObjectPool.Instance.instantiate(NormalDisTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.AOE_LAVA)
					{
						tempTower = ObjectPool.Instance.instantiate(DisAoeTowerPrefab, buildPos, Quaternion.identity);
					}
					else if(objTile.mTileType == Tile.Type.DOT_SNOW)
					{
						tempTower = ObjectPool.Instance.instantiate(DisDotTowerPrefab, buildPos, Quaternion.identity);
					}
					
					else if(objTile.mTileType == Tile.Type.DIS_SCIFI)
					{
						tempTower = ObjectPool.Instance.instantiate(DoubleDisTowerPrefab, buildPos, Quaternion.identity);
					}			
					
					tempTower.GetComponent<Tower>().parentTransform = objTrans;
					
					return;
				}
			}
		}
	}
	
	IEnumerator WaitAndBuild(float waitTime, Tile tile, int towerTypeToBuild) 
	{
        yield return new WaitForSeconds(waitTime);
		AutoBuildTower(tile.gameObject, towerTypeToBuild);
    }
	
	void SwapTowers(Tile oldTile, Tile newTile)
	{
		Tower tempTower = oldTile.transform.GetChild(0).GetComponent<Tower>();
		tempTower.LaunchTower = true;
		tempTower.Swapping = true;
		
		float waitTime = newTile.mFlipInterval + (newTile.mFlipDelayDuration * 1.5f);
		
		if(oldTile.mTileType == Tile.Type.NORMAL_GRASS)
		{
			if(tempTower.mTowerType == Tower.Type.NORMAL_AOE)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 2));	
			}
			else if(tempTower.mTowerType == Tower.Type.NORMAL_DOT)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 1));	
			}
			else if(tempTower.mTowerType == Tower.Type.NORMAL_DIS)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 3));
			}
		}
		else if(oldTile.mTileType == Tile.Type.AOE_LAVA)
		{
			if(tempTower.mTowerType == Tower.Type.DOUBLE_AOE)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 2));	
			}
			else if(tempTower.mTowerType == Tower.Type.DOT_AOE)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 1));	
			}
			else if(tempTower.mTowerType == Tower.Type.DIS_AOE)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 3));	
			}
		}
		else if(oldTile.mTileType == Tile.Type.DOT_SNOW)
		{
			if(tempTower.mTowerType == Tower.Type.DOT_AOE)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 2));
			}
			else if(tempTower.mTowerType == Tower.Type.DOUBLE_DOT)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 1));
			}
			else if(tempTower.mTowerType == Tower.Type.DIS_DOT)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 3));
			}
		}
		else if(oldTile.mTileType == Tile.Type.DIS_SCIFI)
		{
			if(tempTower.mTowerType == Tower.Type.DIS_AOE)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 2));
			}
			else if(tempTower.mTowerType == Tower.Type.DIS_DOT)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 1));	
			}
			else if(tempTower.mTowerType == Tower.Type.DOUBLE_DIS)
			{
				StartCoroutine(WaitAndBuild(waitTime, newTile, 3));
			}
		}
	}
	
	void CheckToFlipTile(GameObject obj)
	{		
		if(obj.tag == "HexGrid")
		{
			Tile oldTile = obj.GetComponent<Tile>();
			Transform objTans = obj.transform;
			
			if(oldTile.mCanBeFlipped && !oldTile.mIsFlipping)
			{				
				GameObject tempTile;
				Tile newTile;
				
				if(enableFlipLavaTile && oldTile.mTileType != Tile.Type.AOE_LAVA)
				{						
					// Instantiate the new below the original, flipped 180 degrees
					tempTile = ObjectPool.Instance.instantiate(gridManager.LavaTilePrefab, objTans.position, Quaternion.Euler(0.0f, 0.0f, 180.0f));
					newTile = tempTile.GetComponent<Tile>();
					
					// Add the original tile to a list to be destroyed
					tileManager.mTileToBeDestroyed.Add(obj);
						
					//Set both tile to flipping
					oldTile.mIsFlipping = true;
					newTile.mIsFlipping = true;
					
					newTile.mHasTowerOnIt = oldTile.mHasTowerOnIt;
					playerManager.Token -= 1;
					
					SwapTowers(oldTile, newTile);
					
					// Disable the button
					enableFlipLavaTile = false;			
				}
				else if(enableFlipSnowTile && oldTile.mTileType != Tile.Type.DOT_SNOW)
				{
					tempTile = ObjectPool.Instance.instantiate(gridManager.SnowTilePrefab, objTans.position, Quaternion.Euler(0.0f, 0.0f, 180.0f));
					newTile = tempTile.GetComponent<Tile>();
					
					// Add the original tile to a list to be destroyed
					tileManager.mTileToBeDestroyed.Add(obj);
					
					//Set both tile to flipping
					oldTile.mIsFlipping = true;
					newTile.mIsFlipping = true;
					
					newTile.mHasTowerOnIt = oldTile.mHasTowerOnIt;
					playerManager.Token -= 1;
					
					SwapTowers(oldTile, newTile);
					
					// Disable the button
					enableFlipSnowTile = false;
				}
				else if(enableFlipScifiTile && oldTile.mTileType != Tile.Type.DIS_SCIFI)
				{
					tempTile = ObjectPool.Instance.instantiate(gridManager.ScifiTilePrefab, objTans.position, Quaternion.Euler(0.0f, 0.0f, 180.0f));
					newTile = tempTile.GetComponent<Tile>();
					
					// Add the original tile to a list to be destroyed
					tileManager.mTileToBeDestroyed.Add(obj);
					
					//Set both tile to flipping
					oldTile.mIsFlipping = true;
					newTile.mIsFlipping = true;
					
					newTile.mHasTowerOnIt = oldTile.mHasTowerOnIt;
					playerManager.Token -= 1;
					
					SwapTowers(oldTile, newTile);
					
					// Disable the button
					enableFlipScifiTile = false;
				}
			}
		}
	}
	
	void CheckToMoveTower(GameObject obj)
	{
		if(obj.tag == "HexGrid")
		{
			Tile objTile = obj.GetComponent<Tile>();
			if(objTile.mHasTowerOnIt)
			{	
				Transform objTower = objTile.transform.GetChild(0);
				
				Tower tower = objTower.GetComponent<Tower>();
				if(!tower.LandTower)
				{
					objTile.mHasTowerOnIt = false;
					
					tower.SetLaunchTower(true);
					
					enableMoveTower = false;
				}
			}
		}
	}
	
	
}
