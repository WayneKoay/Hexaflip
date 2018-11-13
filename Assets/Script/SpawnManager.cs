using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour 
{
	private GridManager gridManager;
	private IngameUIScript ingameUI;
	
	void Start()
	{
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		ingameUI = GameObject.Find("IngameUI").GetComponent<IngameUIScript>();
	}
	
	// Preparation time for the player at the beginning of the game
	public bool preparationTime = true;
	
	[SerializeField] // this is required for non-public fields
	public List<Levels> Level = new List<Levels>();
	
	[System.Serializable]
	public class Levels 
	{
		public string Name;
	    public List<Waves> Wave; 
	}
	
	[System.Serializable]
	public class Waves 
	{
		public bool waveStart = false;
	    public List<Groups> Group;
	}
	
	[System.Serializable]
	public class Groups 
	{
		public float groupSpawnInterval;
	    public GameObject enemyType;
		public int spawnAmount;
		public float enemySpawnInterval;
	}
	
	private float enemySpawnIntervalTimer;
	
	// spawn position
	public Vector3 spawnPosition = Vector3.zero;
	
	// target position
	public Vector3 targetPosition = Vector3.zero;
	
	// gameobject to be spawned
	public GameObject enemy = null;
	
	// current level
	public int currentLevel = 0;
	
	// current wave
	public int currentWave = 0;
	
	// current group
	public int currentGroup = 0;
	
	public int currentSpawn = -1;
	
	// disable spawning
	public bool disableSpawning = false;
	
	public int[,,] enemyList;
	
	private int dropCoinIndex = -1;
	
	// Update is called once per frame
	void Update () 
	{			
		if(ingameUI.endPreparation)
		{
			preparationTime = false;
		}
		
		// Starts the game when preparation time runs out
		if(!preparationTime && !disableSpawning)
		{	
			Level[currentLevel].Wave[currentWave].waveStart = ingameUI.startWave;
			// Starts the wave if wave is activated
			if(Level[currentLevel].Wave[currentWave].waveStart)
			{
				// If group spawn interval is not zero
				if(Level[currentLevel].Wave[currentWave].Group[currentGroup].groupSpawnInterval > 0.0f)
				{
					// Countdown group spawn interval timer
					Level[currentLevel].Wave[currentWave].Group[currentGroup].groupSpawnInterval -= Time.deltaTime;
				}
				// Starts the group spawning if the group spawning interval is less than zero
				if (Level[currentLevel].Wave[currentWave].Group[currentGroup].groupSpawnInterval <= 0.0f)
				{	
					if(dropCoinIndex == -1)
					{
						dropCoinIndex = Random.Range(0, Level[currentLevel].Wave[currentWave].Group[currentGroup].spawnAmount);
					}
					// Interval between each enemy spawn
					// Countdown enemy spawn interval timer
			        enemySpawnIntervalTimer -= Time.deltaTime;
			        if (enemySpawnIntervalTimer <= 0.0f && Level[currentLevel].Wave[currentWave].Group[currentGroup].spawnAmount > 0) 
					{
			            // Spawn
			 			GameObject tempEnemy = ObjectPool.Instance.instantiate(Level[currentLevel].Wave[currentWave].Group[currentGroup].enemyType, spawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
						Level[currentLevel].Wave[currentWave].Group[currentGroup].spawnAmount -= 1;
						currentSpawn += 1;
						
					    tempEnemy.GetComponent<Enemy>().solvedPath = AStarScript.Calculate(gridManager.hexaNodeList[GridManager.Closest(gridManager.hexaNodeList, tempEnemy.transform.position)], gridManager.hexaNodeList[GridManager.Closest(gridManager.hexaNodeList, GameObject.Find("SpawnManager").GetComponent<SpawnManager>().targetPosition)]);
						
			            // Reset enemy spawn interval timer
			            enemySpawnIntervalTimer = Level[currentLevel].Wave[currentWave].Group[currentGroup].enemySpawnInterval;
						
						tempEnemy.GetComponent<Enemy>().tempTokenIndicator = null;
						if(dropCoinIndex == currentSpawn)
						{
							tempEnemy.GetComponent<Enemy>().AddToken();
						}
					}
					
					if(Level[currentLevel].Wave[currentWave].Group[currentGroup].spawnAmount <= 0)
					{
						currentSpawn = -1;
						currentGroup++;
						
						if(currentGroup == Level[currentLevel].Wave[currentWave].Group.Count)
						{
							currentGroup = 0;
							currentWave++;
							ingameUI.startWave = false;
						}
						if(currentWave >= Level[currentLevel].Wave.Count)
						{
							currentGroup = 0;
							currentWave = 0;
							disableSpawning = true;
						}
						if(!disableSpawning)
						{
							dropCoinIndex = Random.Range(0, Level[currentLevel].Wave[currentWave].Group[currentGroup].spawnAmount);
						}
					}
				}
			}
		}
	}    
}
