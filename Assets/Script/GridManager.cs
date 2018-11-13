using UnityEngine;
using System;
using System.Collections.Generic;

public class DrawHelper
{
	public static void DrawCube(Vector3 position, Vector3 size, Color color)
	{
		Vector3 leftFrontDown 	= new Vector3( -size.x / 2.0f, -size.y / 2.0f, -size.z / 2.0f );
		Vector3 rightFrontDown 	= new Vector3( 	size.x / 2.0f, -size.y / 2.0f, -size.z / 2.0f );
		Vector3 rightFrontUp 	= new Vector3( 	size.x / 2.0f, 	size.y / 2.0f, -size.z / 2.0f );
		Vector3 leftFrontUp 	= new Vector3( -size.x / 2.0f, 	size.y / 2.0f, -size.z / 2.0f );
 
		Vector3 leftBackDown 	= new Vector3( -size.x / 2.0f, -size.y / 2.0f, size.z / 2.0f );
		Vector3 rightBackDown 	= new Vector3( 	size.x / 2.0f, -size.y / 2.0f, size.z / 2.0f );
		Vector3 rightBackUp 	= new Vector3( 	size.x / 2.0f, 	size.y / 2.0f, size.z / 2.0f );
		Vector3 leftBackUp 		= new Vector3( -size.x / 2.0f, 	size.y / 2.0f, size.z / 2.0f );
 
		Vector3[] arr = new Vector3[8];
 
		arr[0] = leftFrontDown;
		arr[1] = rightFrontDown;
		arr[2] = rightFrontUp;
		arr[3] = leftFrontUp;
 
		arr[4] = leftBackDown;
		arr[5] = rightBackDown;
		arr[6] = rightBackUp;
		arr[7] = leftBackUp;
 
		for (int i = 0; i < arr.Length; i++)
			arr[i] += position;
 
		for (int i = 0; i < arr.Length; i++)
		{
			for (int j = 0; j < arr.Length; j++)
			{
				if (i != j)
				{
					//Debug.DrawLine(arr[i], arr[j], color);	
				}
			}
		}
	}
}

public class PathNode : MonoBehaviour
{
	public List<PathNode> connections;
	public List<PathNode> originalConnections;
 
	public static int pnIndex;
 
	public Color nodeColor = new Color(0.05f, 0.3f, 0.05f, 0.1f);
 
	public bool Invalid
	{
		get { return (this == null); }
	}
 
	public List<PathNode> Connections
	{
		get { return connections; }
	}
 
	public List<PathNode> OriginalConnections
	{
		get { return originalConnections; }
	}
 
	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
	}
 
	public void Update()
	{
		if(connections == null)
			return;
		for(int i = 0; i < connections.Count; i++)
		{
			if(connections[i] == null)
				continue;
		}
	}
 
	public void Awake()
	{
		if(connections == null)
			connections = new List<PathNode>();
		if(originalConnections == null)
			originalConnections = new List<PathNode>();
 
	}
 
	public static PathNode Spawn(Vector3 inPosition)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		obj.GetComponent<Renderer>().enabled = false;
		obj.name = "hexaNode_" + pnIndex;
		obj.transform.position = inPosition;
		pnIndex++;
 
		PathNode newNode = obj.AddComponent<PathNode>();
		return newNode;
	}	
}

public class GridManager : MonoBehaviour 
{	
	//following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this variable using unity editor
    
    // Tile prefab
    public GameObject GrassTilePrefab = null;
    public GameObject LavaTilePrefab = null;
	public GameObject SnowTilePrefab = null;
	public GameObject ScifiTilePrefab = null;
	public GameObject EmptyTilePrefab = null;
	
	public GameObject NexusPrefab = null;
	public GameObject SpawnerPrefab = null;
	
	//next two variables can also be instantiated using unity editor
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;
 
    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;
	public int[,] Grid;
    //TB of the tile which is the start of the path
    //public TileBehaviour originTileTB = null;
    //TB of the tile which is the end of the path
    //public TileBehaviour destTileTB = null;
 
    public static GridManager instance = null;
	
	public List<PathNode> hexaNodeList;
	public GameObject start;
	public GameObject end;
	public bool reset;
	
	public bool gridCreated;
	int startIndex;
	int endIndex;
 
	int lastEndIndex;
	int lastStartIndex;
 
	bool donePath;
	public List<PathNode> solvedPath = new List<PathNode>();
	public Color nodeColor = new Color(0.05f, 0.3f, 0.05f, 0.1f);
	SpawnManager spawnManager;
 
    void Awake()
    {
        instance = this;
    }
	
	//The grid should be generated on game start
    void Start()
    {		
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
		
		if(Application.loadedLevelName == "Level 1")
		{
			Grid = new int[,]
			{//11 is spawn, 21 is the end, 0 is nothing, 1 is grass
	            {0, 0, 0, 1, 1, 0, 1, 1, 0, 0},
	             {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
	            {0, 0, 0, 1, 1, 0, 1, 1, 0, 0},
	             {0, 1, 1, 0, 1, 1, 0, 1, 1, 0},
	            {0, 21, 1, 1, 1, 1, 1, 1, 1, 11},
				 {0, 1, 1, 0, 1, 1, 0, 1, 1, 0},
	            {0, 0, 0, 1, 1, 0, 1, 1, 0, 0},
	             {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
	            {0, 0, 0, 1, 1, 0, 1, 1, 0, 0},
	             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
		}
		else if(Application.loadedLevelName == "Level 2")
		{
			Grid = new int[,]
			{//11 is spawn, 21 is the end, 0 is nothing, 1 is grass
	              {0,0, 0, 1, 1, 1, 1, 1, 0, 0},
	             {0,0, 1, 1, 1, 1, 1, 1, 0, 0},
	            {0,0, 1, 1, 0, 1, 0, 1, 1, 0},
	             {0,1, 1, 0, 1, 1, 0, 1, 1, 0},
	            {0,21, 1, 1, 1, 1, 1, 1, 1, 11},
				 {0,1, 1, 0, 1, 1, 0, 1, 1, 0},
	            {0,0, 1, 1, 0, 1, 0, 1, 1, 0},
	             {0,0, 1, 1, 1, 1, 1, 1, 0, 0},
	            {0,0, 0, 1, 1, 1, 1, 1, 0, 0},
	             {0,0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
		}
		else if(Application.loadedLevelName == "Level 3")
		{
			Grid = new int[,]
			{//11 is spawn, 21 is the end, 0 is nothing, 1 is grass
	            {21, 1, 1, 1, 1, 1, 1, 1, 1, 1},
	             {0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
	            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
	             {1, 0, 1, 0, 1, 0, 1, 0, 1 ,0},
	            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				 {0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
	            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
	              {1, 0, 1, 0, 1, 0, 1, 0, 1 ,0},
	            {0, 11, 1, 1, 1, 1, 1, 1, 1, 0},
	             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			};
		}
		
        CreateGrid();
		
		//! test mockup path
		/*if(gridCreated)
			return;
		hexaNodeList = PathNode.CreateGrid(Vector3.zero, Vector3.one * 5.0f, new int[] { Grid.GetLength(0), Grid.GetLength(1)}, 0.0f);
		gridCreated = true;*/
		
		//solvedPath = AStarScript.Calculate(hexaNodeList[Closest(hexaNodeList, GameObject.Find("SpawnManager").GetComponent<SpawnManager>().spawnPosition)], hexaNodeList[Closest(hexaNodeList, GameObject.Find("SpawnManager").GetComponent<SpawnManager>().targetPosition)]);
    }

    //Method to initialise Hexagon width and height
    void setSizes(float width, float height)
    {		
        //renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth = width;
		hexHeight = height;
    }
 
    //Method to calculate the position of the first hexagon tile
    //The center of the hex grid is (0,0,0)
    Vector3 calcInitPos()
    {
        Vector3 initPos;
        //the initial position will be in the left upper corner
        initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 
							  0,
           					  gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
 
        return initPos;
    }
 
    //method used to convert hex grid coordinates to game world coordinates
    public Vector3 calcWorldCoord(Vector2 gridPos)
    {
        //Position of the first hex tile
        Vector3 initPos = calcInitPos();
        //Every second row is offset by half of the tile width
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth * 0.5f;
 
        float x =  initPos.x + offset + gridPos.x * hexWidth;
        //Every new line is offset in z direction by 3/4 of the hexagon height
        float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		
        return new Vector3(x, 0, z);
    }
	
    //Finally the method which initialises and positions all the tiles
    void CreateGrid()
    {
        //Game object which is the parent of all the hex tiles
        GameObject hexGridGO = new GameObject("HexGrid");
		GameObject hexGridNode = new GameObject("HexGridNode");
		
		float width = 0.0f;
		float height =  0.0f;
 
        for (int y = 0; y < gridHeightInHexes; y++)
        {
            for (int x = 0; x < gridWidthInHexes; x++)
            {
                //GameObject assigned to Hex public variable is cloned
				GameObject Hex = null;
				
				int tempGridValue = Grid[y, x];
				
				if(Grid[y, x] >= 0)
				{
					Grid[y, x] %= 10;
				}
				
				if (Grid[y, x] == 1)
				{
                	Hex = ObjectPool.Instance.instantiate(GrassTilePrefab);
					if(width == 0.0f && height == 0.0f)
					{
				        width = Hex.GetComponent<Renderer>().bounds.size.x;
				        height = Hex.GetComponent<Renderer>().bounds.size.z;						
						setSizes(width, height);
					}
				}
				if (Grid[y, x] == 2)
				{
                	Hex = ObjectPool.Instance.instantiate(LavaTilePrefab);
					if(width == 0.0f && height == 0.0f)
					{
				        width = Hex.GetComponent<Renderer>().bounds.size.x;
				        height = Hex.GetComponent<Renderer>().bounds.size.z;						
						setSizes(width, height);
					}
				}
				if (Grid[y, x] == 3)
				{
                	Hex = ObjectPool.Instance.instantiate(SnowTilePrefab);
					if(width == 0.0f && height == 0.0f)
					{
				        width = Hex.GetComponent<Renderer>().bounds.size.x;
				        height = Hex.GetComponent<Renderer>().bounds.size.z;						
						setSizes(width, height);
					}
				}
				if (Grid[y, x] == 4)
				{
                	Hex = ObjectPool.Instance.instantiate(ScifiTilePrefab);
					if(width == 0.0f && height == 0.0f)
					{
				        width = Hex.GetComponent<Renderer>().bounds.size.x;
				        height = Hex.GetComponent<Renderer>().bounds.size.z;						
						setSizes(width, height);
					}
				}
				if (Grid[y, x] == 0)
				{
                	Hex = ObjectPool.Instance.instantiate(EmptyTilePrefab);
					if(width == 0.0f && height == 0.0f)
					{
				        width = Hex.GetComponent<Renderer>().bounds.size.x;
				        height = Hex.GetComponent<Renderer>().bounds.size.z;						
						setSizes(width, height);
					}
				}
				
				
				//Current position in grid
				if(Hex)
				{
	                Vector2 gridPos = new Vector2(x, y);
	                Hex.transform.position = calcWorldCoord(gridPos);
	                Hex.transform.parent = hexGridGO.transform;
				
					PathNode tempNode = PathNode.Spawn(Hex.transform.position);
					tempNode.transform.parent = hexGridNode.transform;
	 
					hexaNodeList.Add(tempNode);
				}
				
				if(tempGridValue >= 10)
				{
					if(tempGridValue < 20)
					{
						GameObject.Find("SpawnManager").GetComponent<SpawnManager>().spawnPosition = calcWorldCoord(new Vector2(x, y));
						ObjectPool.Instance.instantiate(SpawnerPrefab, calcWorldCoord(new Vector2(x, y)), Quaternion.identity);
					}
					else if(tempGridValue < 30)
					{
						GameObject.Find("SpawnManager").GetComponent<SpawnManager>().targetPosition = calcWorldCoord(new Vector2(x, y));
						ObjectPool.Instance.instantiate(NexusPrefab, calcWorldCoord(new Vector2(x, y)), Quaternion.identity);
					}
				}
			}
		}
		
		for (int y = 0; y < gridHeightInHexes; y++)
	        {
	            for (int x = 0; x < gridWidthInHexes; x++)
	            {
					int currentIndex = (y * gridWidthInHexes) + x;
					List<int> neighbourList = new List<int>();
					PathNode currentNode = hexaNodeList[currentIndex];
					
					if(AStarScript.Invalid(currentNode)) continue;
					
					//Check left neighbour
					if(x > 0)
					{
						neighbourList.Add((y * gridWidthInHexes) + x - 1);
					}
					//Check right neighbour
					if(x != gridWidthInHexes - 1)
					{
						neighbourList.Add((y * gridWidthInHexes) + x + 1);
					}
					
					// Check odd row
					if(y%2 == 0)
					{
						//Check top left neighbour
						if(x != 0 && y != 0)
						{
							neighbourList.Add(((y - 1) * gridWidthInHexes) + x  - 1);
						}
						//Check top right neighbour
						if(y != 0)
						{
							neighbourList.Add(((y - 1) * gridWidthInHexes) + x);
						}
						//Check bottom left neighbour
						if(x != 0 && y != gridHeightInHexes - 1)
						{
							neighbourList.Add(((y + 1) * gridWidthInHexes) + x - 1);
						}
						//Check bottom right neighbour
						if(y != gridHeightInHexes - 1)
						{
							neighbourList.Add(((y + 1) * gridWidthInHexes) + x);
						}
					}
					//Check even row
					else
					{
						//Check top left neighbour
						if(y != 0)
						{
							neighbourList.Add(((y - 1) * gridWidthInHexes) + x);
						}
						//Check top right neighbour
						if(y != 0 && x != gridWidthInHexes - 1)
						{
							neighbourList.Add(((y - 1) * gridWidthInHexes) + x + 1);
						}
						//Check bottom left neighbour
						if(y != gridHeightInHexes - 1)
						{
							neighbourList.Add(((y + 1) * gridWidthInHexes) + x);
						}
						//Check bottom right neighbour
						if(y != gridHeightInHexes - 1  && x != gridWidthInHexes - 1)
						{
							neighbourList.Add(((y + 1) * gridWidthInHexes) + x + 1);
						}
					}	
					
					for(int i = 0; i < neighbourList.Count; i++)
					{
						PathNode connectedNode = hexaNodeList[neighbourList[i]];
						if(AStarScript.Invalid(connectedNode))
							continue;
						currentNode.Connections.Add(connectedNode);
						currentNode.OriginalConnections.Add(connectedNode);
					}
				}
			}
			
			for (int y = 0; y < gridHeightInHexes; y++)
	        {
	            for (int x = 0; x < gridWidthInHexes; x++)
	            {
					if(Grid[y, x] == 0)
					{
						int currentIndex = (y * gridWidthInHexes) + x;
						PathNode nodeToDisable = hexaNodeList[currentIndex];
						
						DisableNode(nodeToDisable.Position);
					
						for(int i=0; i<nodeToDisable.Connections.Count; i++)
						{
							PathNode neighbourNode = nodeToDisable.Connections[i];
							for(int j=0; j<neighbourNode.Connections.Count; j++)
							{
								if(neighbourNode.Connections[j] == nodeToDisable)
								{
									neighbourNode.Connections.Remove(nodeToDisable);
									j--;
								}
							}
						}
						nodeToDisable.Connections.Clear();
					}
				}
			}
	}
	 
	public static int Closest(List<PathNode> inNodes, Vector3 toPoint)
	{
		int closestIndex = 0;
		float minDist = float.MaxValue;
		for(int i = 0; i < inNodes.Count; i++)
		{
			if(AStarScript.Invalid(inNodes[i]))
				continue;
			float thisDist = Vector3.Distance(toPoint, inNodes[i].Position);
			if(thisDist > minDist)
				continue;
 
			minDist = thisDist;
			closestIndex = i;
		}
 
		return closestIndex;
	}
	 
	public static int ClosestExceptCurrentNode(List<PathNode> inNodes, PathNode currentNode)
	{
		Vector3 toPoint = currentNode.transform.position;
		int closestIndex = 0;
		float minDist = float.MaxValue;
		for(int i = 0; i < inNodes.Count; i++)
		{
			if(AStarScript.Invalid(inNodes[i]) ||
				currentNode == inNodes[i] ||
				inNodes[i].Connections.Count == 0)
			{
				continue;
			}
			float thisDist = Vector3.Distance(toPoint, inNodes[i].Position);
			if(thisDist > minDist)
				continue;
 
			minDist = thisDist;
			closestIndex = i;
		}
 
		return closestIndex;
	}
 
 
	public void Draw(int startPoint, int endPoint, Color inColor)
	{
		//Debug.DrawLine(hexaNodeList[startPoint].Position, hexaNodeList[endPoint].Position, inColor);
	}
	
	void Update()
	{
		for(int i = 0; i < hexaNodeList.Count; i++)
		{
			for(int j = 0; j < hexaNodeList[i].Connections.Count; j++)
			{
				//Debug.DrawLine(hexaNodeList[i].Position + new Vector3(0.0f, 3.0f, 0.0f), hexaNodeList[i].Connections[j].Position + new Vector3(0.0f, 3.0f, 0.0f), Color.blue * new Color(1.0f, 1.0f, 1.0f, 0.5f)); 		
			}
		}	
				
		//Draw path	
		for(int i = 0; i < solvedPath.Count - 1; i++)
		{
			/*if(AStarScript.Invalid(solvedPath[i]) || AStarScript.Invalid(solvedPath[i + 1]))
			{
				reset = true;
 
				return;
			}*/
			//Debug.DrawLine(solvedPath[i].Position + new Vector3(0.0f, 1.0f, 0.0f), solvedPath[i + 1].Position + new Vector3(0.0f, 1.0f, 0.0f), Color.red * new Color(1.0f, 1.0f, 1.0f, 0.5f)); 
		}
	}
	
	public bool CheckPathExist(Vector3 position)
	{
		int nodeToDisableIndex = Closest(hexaNodeList, position);
		PathNode nodeToDisable = hexaNodeList[nodeToDisableIndex];
		
		List<PathNode> oldPathNode = new List<PathNode>();
		
		if(position == spawnManager.targetPosition || position == spawnManager.spawnPosition)
		{
			return false;	
		}
		
		//Backup neighbour list
		for(int i=0; i<nodeToDisable.Connections.Count; i++)
		{
			oldPathNode.Add(nodeToDisable.Connections[i]);
		}
		//Disconnect all link from current node to neighhour nodes
		nodeToDisable.Connections.Clear();
		
		//Check if path is valid
		//	Allow tower to be built if valid
		//	Don't allow tower to be built if invalid
		List<PathNode> testPath = (List<PathNode>)AStarScript.Calculate(hexaNodeList[Closest(hexaNodeList, GameObject.Find("SpawnManager").GetComponent<SpawnManager>().spawnPosition)], hexaNodeList[Closest(hexaNodeList, GameObject.Find("SpawnManager").GetComponent<SpawnManager>().targetPosition)]);
		
		for (int i = 0; i < oldPathNode.Count; ++i)
		{
			PathNode tempOldPathNode = oldPathNode[i];
			nodeToDisable.Connections.Add(tempOldPathNode);
		}
		
		if (testPath != null)
		{			
			return true;
		}
		return false;	
	}
	
	public void DisableNode(Vector3 position)
	{
		int nodeToDisableIndex = Closest(hexaNodeList, position);
		PathNode nodeToDisable = hexaNodeList[nodeToDisableIndex];
		
		List<PathNode> oldPathNode = new List<PathNode>();
		
		for(int i=0; i<nodeToDisable.Connections.Count; i++)
		{
			oldPathNode.Add(nodeToDisable.Connections[i]);
		}
		
		//Disconnect all link from neighbour nodes to current node
		for(int i=0; i<nodeToDisable.Connections.Count; i++)
		{
			PathNode neighbourNode = nodeToDisable.Connections[i];
			for(int j=0; j<neighbourNode.Connections.Count; j++)
			{
				if(neighbourNode.Connections[j] == nodeToDisable)
				{
					neighbourNode.Connections.Remove(nodeToDisable);
					j--;
				}
			}
		}
		//Disconnect all link from current node to neighhour nodes
		nodeToDisable.Connections.Clear();
		
		GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemyList.Length; ++i)
		{
			Enemy enemyScript = enemyList[i].GetComponent<Enemy>();
			// If there where any nodes to be disabled
			// Enemies are force to go to the next node
			if(enemyScript.solvedPath[enemyScript.mPathIndex] == nodeToDisable)
			{
				enemyScript.mForceGoToNext = true;
				enemyScript.solvedPath = AStarScript.Calculate(ShortestNextPathNodeFromNeighbour(oldPathNode), hexaNodeList[Closest(hexaNodeList, spawnManager.targetPosition)]);
			}
			else
			{
				
				enemyScript.solvedPath = AStarScript.Calculate(enemyScript.solvedPath[enemyScript.mPathIndex], hexaNodeList[Closest(hexaNodeList, spawnManager.targetPosition)]);
			}
			enemyScript.mPathIndex = 0;
		}
		
		Array.Clear(enemyList, 0, enemyList.Length);
	}
	
	public void EnableNode(Vector3 position)
	{
		int nodeToEnableIndex = Closest(hexaNodeList, position);
		PathNode nodeToEnable = hexaNodeList[nodeToEnableIndex];
		
		for(int i=0; i<nodeToEnable.OriginalConnections.Count; i++)
		{
			if(nodeToEnable.OriginalConnections[i].Connections.Count > 0)
			{
				nodeToEnable.Connections.Add(nodeToEnable.OriginalConnections[i]);
				nodeToEnable.OriginalConnections[i].Connections.Add(nodeToEnable);
			}
		}
		
	}
	public PathNode ShortestNextPathNodeFromNeighbour(List<PathNode> currentNode)
	{
		
		int chosenNeighbourIndex = 0;
		int shortestPathValue = 100000;
		List<PathNode> shortestPath;
		 
		for(int i=0; i<currentNode.Count; i++)
		{
			shortestPath = AStarScript.Calculate(currentNode[i], hexaNodeList[Closest(hexaNodeList, spawnManager.targetPosition)]);
			int tempPathValue = 1000000;
			if(shortestPath != null)
			{
				tempPathValue = shortestPath.Count;
			}
			if(tempPathValue < shortestPathValue)
			{
				shortestPathValue = tempPathValue;
				chosenNeighbourIndex = i;
			}
		}
		return currentNode[chosenNeighbourIndex];
	}
}
