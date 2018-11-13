using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
	public enum Type
	{
		NORMAL_AOE = 0,
		NORMAL_DIS,
		NORMAL_DOT,
		DOUBLE_AOE,
		DOUBLE_DIS,
		DOUBLE_DOT,
		DIS_AOE,
		DIS_DOT,
		DOT_AOE,
		TOTAL
	}
	
	public Type mTowerType;
	
	public GameObject myProjectile;
	public float reloadTime;
	public float turnSpeed;
	public float firePauseTime;
	//public GameObject muzzleEffect;
	public float errorAmount;
	public Transform myTarget;
	public Transform[] muzzlePosition;
	public Transform turretBody;
	
	private float nextFireTime;
	private float nextMovetime;
	//private Quaternion desiredRotation;
	//private float aimError;
	
	public float LaunchTargetPos;
	private float LandTargetPos;
	
	public float LaunchInterval;
	public float LaunchSpeed = 0.0f;
	
	public float LandInterval;
	public float LandSpeed = 0.0f;
	
	public bool LaunchTower = false;
	public bool LandTower = true;
	
	public bool Swapping = false;
	
	public float fov;
	
	private Transform mTransform;
	
	public Transform parentTransform;
	
	private PlayerManager playerManager;
	private GridManager gridManager;
	
	StatusEffect targettedEnemy;
	Enemy enemy;

	void OnEnable()
	{
		SetLandTower(true);
	}
	
	void Start()
	{
		mTransform = transform;
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		
		LaunchTargetPos = mTransform.position.y + 300.0f;
		LandTargetPos = mTransform.position.y - 300.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(myTarget != null)
		{
			enemy  = myTarget.gameObject.GetComponent<Enemy>();
			targettedEnemy = enemy.gameObject.GetComponent<StatusEffect>();
		}
		
		if (myTarget != null && 
				(
					!myTarget.gameObject.activeSelf || myTarget.gameObject.GetComponent<Enemy>().IsDead || 
					(mTowerType == Type.NORMAL_DIS && targettedEnemy.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_THREE) ||
					(mTowerType == Type.NORMAL_DOT && targettedEnemy.statusTypeDot == StatusEffect.StatusType.NORMAL_DOT) ||
					(mTowerType == Type.DOUBLE_DOT && targettedEnemy.statusTypeDot == StatusEffect.StatusType.DOUBLE_DOT) ||
					(mTowerType == Type.DOUBLE_DIS && targettedEnemy.statusTypeDis == StatusEffect.StatusType.STUN)
				)
			)
			myTarget = null;
		
		if (myTarget)
		{
			float angle = 0.0f;
			if (Time.time >= nextMovetime && !LandTower && !LaunchTower)
			{
				//turretBody.transform.LookAt(myTarget.position);
				//CalculateAimPosition(myTarget.position);
				Vector3 targetDir = myTarget.position - turretBody.position;
				
				turretBody.rotation = Quaternion.Slerp(turretBody.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * turnSpeed);

				angle = Vector3.Angle(targetDir, turretBody.forward);
			}
			
			if (Time.time >= nextFireTime && angle < fov && !LandTower && !LaunchTower)
			{
				FireProjectile();
			}
		}
		
		//Debug.Log(mTransform.parent);
		
		if(LandTower)
		{
			Land();	
		}
		
		if(LaunchTower)
		{
			Launch();	
		}
	}
	
	/*void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy" && myTarget == null)
		{
			//nextFireTime = Time.time + (reloadTime * 0.5f);
			myTarget = other.gameObject.transform;
		}
	}*/
	void OnTriggerStay(Collider other)
	{		
		if (other.gameObject.tag == "Enemy")
		{
			Enemy target = other.gameObject.GetComponent<Enemy>();
			StatusEffect status = other.gameObject.GetComponentInChildren<StatusEffect>();
			
			if (myTarget == null && !target.IsDead)
			{
				if(mTowerType == Type.NORMAL_DIS && status.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_THREE ||
					(mTowerType == Type.NORMAL_DOT && status.statusTypeDot == StatusEffect.StatusType.NORMAL_DOT) ||
					(mTowerType == Type.DOUBLE_DOT && status.statusTypeDot == StatusEffect.StatusType.DOUBLE_DOT) ||
					(mTowerType == Type.DOUBLE_DIS && status.statusTypeDis == StatusEffect.StatusType.STUN)
					)
				{
					return;
				}
				else
				{
					//nextFireTime = Time.time + (reloadTime * 0.5f);
					myTarget = other.gameObject.transform;
				}
			}
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.transform == myTarget)
		{
			myTarget = null;
		}
	}
	/*
	void CalculateAimPosition(Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x + aimError, targetPos.y + aimError, targetPos.z + aimError);
		desiredRotation = Quaternion.LookRotation(aimPoint);
	}
	
	void CalculateAimError()
	{
		aimError = Random.Range(-errorAmount, errorAmount);	
	}
	*/
	void FireProjectile()
	{
		GetComponent<AudioSource>().Play();
		nextFireTime = Time.time + reloadTime;
		nextMovetime = Time.time + firePauseTime;
		//CalculateAimError();
		
		foreach (Transform theMuzzlePos in muzzlePosition)	
		{
			GameObject newProjectile = ObjectPool.Instance.instantiate(myProjectile, theMuzzlePos.position, theMuzzlePos.rotation);
			newProjectile.GetComponent<ProjectileBase>().aimAt(myTarget);
		}
	}
	
	void Launch()
	{
		if(mTransform.position.y < 300)
		{
			float pos = Mathf.SmoothDamp(mTransform.position.y, LaunchTargetPos, ref LaunchSpeed, LaunchInterval);
			
			mTransform.position = new Vector3(mTransform.position.x, pos, mTransform.position.z);
		}
		else
		{
			LaunchTower = false;	
			ObjectPool.Instance.destroy(gameObject);
			
			if(!Swapping)
			{
				playerManager.Token += 1;	
			}
		}
	}
	
	void Land()
	{
		if(mTransform.position.y > 1.0f && !parentTransform.GetComponent<Tile>().mIsFlipping)
		{
			float pos = Mathf.SmoothDamp(mTransform.position.y, LandTargetPos, ref LandSpeed, LandInterval);
			
			mTransform.position = new Vector3(mTransform.position.x, pos, mTransform.position.z);
			
			mTransform.parent = parentTransform;
			Tile tempTile = mTransform.parent.GetComponent<Tile>();
			
			gridManager.DisableNode(tempTile.transform.position);
			tempTile.mCanBeBuiltOn = false;
			tempTile.mHasTowerOnIt = true;
			tempTile.mCanBeFlipped = true;
		}
		else
		{
			LandTower = false;	
		}
	}
	
	public void SetLaunchTower(bool launch)
	{
		if(launch)
		{
			LaunchSpeed = 0;
			gridManager.EnableNode(mTransform.parent.position);
			mTransform.parent.GetComponent<Tile>().enableDelay = true;
			mTransform.parent.DetachChildren();
			LaunchTower = true;
		}
	}
	
	public void SetLandTower(bool land)
	{
		if(land)
		{
			LandSpeed = 0;
			LandTower = true;
		}
	}
}