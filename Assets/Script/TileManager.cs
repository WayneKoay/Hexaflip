using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour 
{
	public float mFlipSpeed = 1.0f;
	
	public List<GameObject> mTileSwappingList = new List<GameObject>();
	public List<GameObject> mTileToBeDestroyed = new List<GameObject>();
	
	private PlayerManager playerManager;
	private GridManager gridManager;
	
	private Tile TileSwappingListGC;
	private Tile TileToBeDestroyedGC;
	
	// Use this for initialization
	void Start () 
	{
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{		
		/*
		for (int i = 0; i < mTileSwappingList.Count; ++i)
		{
			TileSwappingListGC = mTileSwappingList[i].GetComponent<Tile>();
			
			if (!TileSwappingListGC.mHasTowerOnIt)
			{
				mTileSwappingList[i].transform.RotateAround(mTileSwappingList[i].transform.position, Vector3.forward, mFlipSpeed * Time.deltaTime);
				TileSwappingListGC.mFlippedAngle += mFlipSpeed * Time.deltaTime;
				
				if(TileSwappingListGC.mFlippedAngle > 180.0f)
				{
					for (int j = 0; j < mTileToBeDestroyed.Count; ++j)
					{
						if (mTileSwappingList[i] == mTileToBeDestroyed[j])
						{
							TileToBeDestroyedGC = mTileToBeDestroyed[j].GetComponent<Tile>();
							/*
							if (TileToBeDestroyedGC.mTileType == Tile.Type.DOT_SNOW)
							{
								playerManager.mDotTile += 1;
							}
							else if (TileToBeDestroyedGC.mTileType == Tile.Type.AOE_LAVA)
							{
								playerManager.mAoeTile += 1;
							}	
							else if (TileToBeDestroyedGC.mTileType == Tile.Type.DIS_SCIFI)
							{
								playerManager.mDisTile += 1;
							}
							
							ObjectPool.Instance.destroy(mTileToBeDestroyed[j]);
							mTileToBeDestroyed.Remove(mTileToBeDestroyed[j]);
						}
					}
					TileSwappingListGC.mIsToBeFlipped = false;
					TileSwappingListGC.mFlippedAngle = 0.0f;
					//TileSwappingListGC.transform.rotation
					gridManager.EnableNode(mTileSwappingList[i].transform.position);			
					mTileSwappingList.Remove(mTileSwappingList[i]);
					i--;
					
				}
			}
		}*/
	}
}
