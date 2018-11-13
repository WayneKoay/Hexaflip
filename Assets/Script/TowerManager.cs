using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour {
	
	private TileManager tileManager;
	private PlayerManager playerManager;
	
	public List<GameObject> mTowerList = new List<GameObject>();
	
	public GameObject tempTower = null;
	
	private Tower tempTowerGC;
	private Tile TileSwappingListGC;
	
	// Use this for initialization
	void Start () 
	{
		tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if(tileManager.mTileSwappingList.Count > 0)
		{
			for (int i = 0; i < tileManager.mTileSwappingList.Count; ++i)
			{
				TileSwappingListGC = tileManager.mTileSwappingList[i].GetComponent<Tile>();
				
				if (TileSwappingListGC.mHasTowerOnIt && TileSwappingListGC.mIsToBeFlipped)
				{
					tempTower = FindTowerOnTile(mTowerList, tileManager.mTileSwappingList[i].transform.position);
					tempTowerGC = tempTower.GetComponent<Tower>();
					
					mTowerList.Remove(tempTower);
					//Debug.Log(mTowerList.Count);
					
					if  (tempTowerGC.mTowerType == Tower.Type.NORMAL_DOT ||
						(tempTowerGC.mTowerType == Tower.Type.DOUBLE_DOT && TileSwappingListGC.mTileType == Tile.Type.DOT_SNOW) ||
					  	(tempTowerGC.mTowerType == Tower.Type.DIS_DOT && TileSwappingListGC.mTileType == Tile.Type.DIS_SCIFI) ||
					  	(tempTowerGC.mTowerType == Tower.Type.DOT_AOE && TileSwappingListGC.mTileType == Tile.Type.AOE_LAVA)
					  	)
					{
						playerManager.mDotTower += 1;
					}
					else if (tempTowerGC.mTowerType == Tower.Type.NORMAL_AOE ||
					  		(tempTowerGC.mTowerType == Tower.Type.DOUBLE_AOE && TileSwappingListGC.mTileType == Tile.Type.AOE_LAVA) ||
					  		(tempTowerGC.mTowerType == Tower.Type.DIS_AOE && TileSwappingListGC.mTileType == Tile.Type.DIS_SCIFI) ||
					  		(tempTowerGC.mTowerType == Tower.Type.DOT_AOE && TileSwappingListGC.mTileType == Tile.Type.DOT_SNOW)
				  			)
					{
						playerManager.mAoeTower += 1;
					}
					else if (tempTowerGC.mTowerType == Tower.Type.NORMAL_DIS ||
					  		(tempTowerGC.mTowerType == Tower.Type.DOUBLE_DIS && TileSwappingListGC.mTileType == Tile.Type.DIS_SCIFI) ||
					  		(tempTowerGC.mTowerType == Tower.Type.DIS_DOT && TileSwappingListGC.mTileType == Tile.Type.DOT_SNOW) ||
					  		(tempTowerGC.mTowerType == Tower.Type.DIS_AOE && TileSwappingListGC.mTileType == Tile.Type.AOE_LAVA)
					  		)
					{
						playerManager.mDisTower += 1;
					}
					
					tempTower.transform.position = new Vector3(0,-100,0);
					TileSwappingListGC.mHasTowerOnIt = false;
					tileManager.mTileSwappingList[i+1].GetComponent<Tile>().mHasTowerOnIt = false;
				}
			}
		}
		*/
	}
	
	/*
	public GameObject FindTowerOnTile(List<GameObject> towerList, Vector3 tilePos)
	{
		GameObject tempTower = null;
		float minDist = float.MaxValue;
		for(int i = 0; i < towerList.Count; i++)
		{
			if(towerList[i] == null)
				continue;
			float thisDist = Vector3.Distance(tilePos, towerList[i].transform.position);
			if(thisDist > minDist)
				continue;
 			//Debug.Log(minDist + " : " + thisDist);
			
			minDist = thisDist;
 			//Debug.Log(minDist + " : " + thisDist);
			tempTower = towerList[i];
		}
 
		return tempTower;
	}
	*/
}
