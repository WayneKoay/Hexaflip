using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{
	public enum Type
	{
		NORMAL_GRASS = 0,
		DOT_SNOW,
		AOE_LAVA,
		DIS_SCIFI,
		TOTAL
	}
	
	public Type mTileType = Type.NORMAL_GRASS;
	
	public bool mCanBeBuiltOn = true;
	public bool mCanBeFlipped = false;
	public bool mIsToBeFlipped = false;
	public bool mHasTowerOnIt = false;
	public bool mIsFlipping = false;
	
	public float mFlippedAngle = 0.0f;
	
	Transform mTransform;
	
	
	Quaternion mNonFlippedRotation;
	Quaternion mFlippedRotation;
	Quaternion mFullFlippedRotation;
	public float mFlipDelayDuration = 0.5f;
	public float mFlipDelayElasped = 0.0f;
	public float mFlipInterval = 2.0f;
	public float mFlipElapsed = 0.0f;
	public bool mFacingUp = true;
	
	private PlayerManager playerManager;
	private GridManager gridManager;
	private TileManager tileManager;
	
	public bool enableDelay = false;
	public float enableDelayTimer = 0.0f;
	public float enableDelayDuration = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
		
		mTransform = transform;
		mNonFlippedRotation = mTransform.rotation;
		mFlippedRotation = mNonFlippedRotation * Quaternion.AngleAxis(180.0f, Vector3.forward);
		mFullFlippedRotation = mNonFlippedRotation * Quaternion.AngleAxis(360.0f, Vector3.forward);
	}
	
	public void ChangeTileType(Type type)
	{
		// start flipping.
		mIsFlipping = true;
		mFacingUp = !mFacingUp;
		mFlipElapsed = 0.0f;
	}
	
	public void DestroyFlippedTile()
	{
		for (int j = 0; j < tileManager.mTileToBeDestroyed.Count; ++j)
		{
			if (tileManager.mTileToBeDestroyed[j])
			{
				ObjectPool.Instance.destroy(tileManager.mTileToBeDestroyed[j]);
				tileManager.mTileToBeDestroyed.Remove(tileManager.mTileToBeDestroyed[j]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(enableDelay)
		{
			enableDelayTimer += Time.deltaTime;
			if(enableDelayTimer > enableDelayDuration)
			{
				enableDelay = false;
				mHasTowerOnIt = false;
				mCanBeBuiltOn = true;
				mCanBeFlipped = false;
				
				enableDelayTimer = 0.0f;
				enableDelay = false;
			}
		}
		
		if (!mIsFlipping) return;
		
		mFlipDelayElasped += Time.deltaTime;
		
		if(mFlipDelayElasped > mFlipDelayDuration)
		{
			mFlipElapsed += Time.deltaTime;
			if (!mFacingUp)
			{
				if (mFlipElapsed > mFlipInterval)
				{
					mTransform.rotation = mNonFlippedRotation;
					mIsFlipping = false;
					DestroyFlippedTile();
					mFlipDelayElasped = 0;
					mFlipElapsed = 0;
					mFacingUp = !mFacingUp;
					return;
				}
				mTransform.rotation = Quaternion.Slerp(mFlippedRotation, mFullFlippedRotation, mFlipElapsed / mFlipInterval);
			}
			else
			{
				if (mFlipElapsed > mFlipInterval)
				{
					mTransform.rotation = mFlippedRotation;
					mIsFlipping = false;
					DestroyFlippedTile();
					mFlipDelayElasped = 0;
					mFlipElapsed = 0;
					mFacingUp = !mFacingUp;
					return;
				}
				mTransform.rotation = Quaternion.Slerp(mNonFlippedRotation, mFlippedRotation, mFlipElapsed / mFlipInterval);
			}
		}
	}
	
	
}
