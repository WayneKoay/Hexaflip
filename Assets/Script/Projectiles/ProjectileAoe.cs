using UnityEngine;
using System.Collections;

public class ProjectileAoe : ProjectileBase 
{
	public Transform mTarget;
	
	public float radius;
	
	public float NormalAoeDamage;
	public GameObject NormalAoeExplosion;
	
	public float DoubleAoeDamage;
	public GameObject DoubleAoeExplosion;
		
	public float AoeDisablerDuration;
	public float AoeDisablerDurationElapsed = 0;
	public float AoeDisablerDamage;
	public float AoeDisablerSlow;
	public GameObject AoeDisablerExplosion;
	
	public float DotAoeDuration;
	public float DotAoeDurationElapsed = 0;
	public float DotAoeInterval;
	public float DotAoeIntervalElapsed = 0;
	public float DotAoeDamage;
	public GameObject DotAoeExplosion;
	
	RaycastHit hitInfo;
	Collider[] oldEnemies;
	Collider[] enemies;
	
	public bool isStopMovement = false;
	public bool isEnableUpdate = false;
	public bool isFirstHit = true;
	
	public override void aimAt(Transform target)
	{
		mTarget = target;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	/*void OnEnable()
	{
		
		
		Debug.Log("ENABLE" + AoeDisablerDurationElapsed);
	}*/
	
	// Update is called once per frame
	protected override void Update() 
	{		
		if (!isStopMovement)
		{
			move();
		}
		
		if (gameObject.tag == "ProjectileDoTAoE" && isEnableUpdate)
		{
			Update_DoTAoE();
		}
		else if (gameObject.tag == "ProjectileAoEDis" && isEnableUpdate)
		{
			Update_AoEDisabler();
		}
	}
	
	void Explode_NormalAoE()
	{
		LayerMask layerMask = LayerMask.NameToLayer("Enemy");
		layerMask = ~layerMask;
		enemies = Physics.OverlapSphere(transform.position, radius, layerMask);
		
		foreach(Collider enemy in enemies)
		{
		  	if(enemy.tag == "Enemy")
			{
				enemy.gameObject.GetComponent<Enemy>().mHealth -= NormalAoeDamage;
			}
		}
		
		ObjectPool.Instance.instantiate(NormalAoeExplosion, gameObject.transform.position, Quaternion.identity);
		
		ResetStats();
		ObjectPool.Instance.destroy(gameObject);
	}
	
	void Explode_AoEDisabler()
	{
		LayerMask layerMask = LayerMask.NameToLayer("Enemy");
		layerMask = ~layerMask;
		enemies = Physics.OverlapSphere(transform.position, radius, layerMask);
		
		AoeDisablerDurationElapsed = 0;
		
		oldEnemies = enemies;
		isEnableUpdate = true;
		isFirstHit = false;
		GetComponent<Renderer>().enabled = false;
			
		ObjectPool.Instance.instantiate(AoeDisablerExplosion, gameObject.transform.position, Quaternion.identity);
		
		foreach(Collider enemy in enemies)
		{
		  	if(enemy.tag == "Enemy")
			{
				enemy.gameObject.GetComponent<Enemy>().mHealth -= AoeDisablerDamage;
			}
		}
		
	}
	
	void Update_AoEDisabler()
	{
		LayerMask layerMask = LayerMask.NameToLayer("Enemy");
		layerMask = ~layerMask;
		enemies = Physics.OverlapSphere(transform.position, radius, layerMask);
		
		foreach(Collider oldEnemy in oldEnemies)
		{
			bool isEscape = true;
			foreach(Collider enemy in enemies)
			{
				if (oldEnemy == enemy)
				{
					isEscape = false;
					break;
				}
			}
			if (isEscape && oldEnemy.tag == "Enemy")
			{
				Enemy tempEnemy = oldEnemy.gameObject.GetComponent<Enemy>(); 
				tempEnemy.mSpeed = tempEnemy.OriginalSpeed;				
			}
		}
		oldEnemies = enemies;
		
		foreach(Collider enemy in enemies)
		{
			Enemy tempEnemy = enemy.gameObject.GetComponent<Enemy>(); 
		  	if(enemy.tag == "Enemy" && !tempEnemy.IsDead && tempEnemy.gameObject.GetComponent<StatusEffect>().statusTypeDis != StatusEffect.StatusType.STUN)
			{
				tempEnemy.mSpeed = tempEnemy.OriginalSpeed * (1 - AoeDisablerSlow);
			}
		}
		
		AoeDisablerDurationElapsed += Time.deltaTime;
		
		if (AoeDisablerDurationElapsed > AoeDisablerDuration)
		{	
			foreach(Collider enemy in enemies)
			{
			  	if(enemy.tag == "Enemy" && !enemy.gameObject.GetComponent<Enemy>().IsDead)
				{
					Enemy tempEnemy = enemy.gameObject.GetComponent<Enemy>(); 
					tempEnemy.mSpeed = tempEnemy.OriginalSpeed;					
				}
			}
			
			ResetStats();
			
			ObjectPool.Instance.destroy(gameObject);	
		}
	}
	
	void Explode_DoTAoE()
	{
		isEnableUpdate = true;
		GetComponent<Renderer>().enabled = false;
	}
	
	void Update_DoTAoE()
	{
		LayerMask layerMask = LayerMask.NameToLayer("Enemy");
		layerMask = ~layerMask;
		enemies = Physics.OverlapSphere(transform.position, radius, layerMask);
		
		DotAoeDurationElapsed += Time.deltaTime;
		
		Instantiate(DotAoeExplosion, gameObject.transform.position, Quaternion.identity);
		
		//Debug.Log(DotAoeDurationElapsed);
		if (DotAoeDurationElapsed < DotAoeDuration)
		{
			DotAoeIntervalElapsed += Time.deltaTime;
			if (DotAoeIntervalElapsed > DotAoeInterval)
			{
				for(int i = 0; i < enemies.Length; ++i)
				{
				  	if(enemies[i].tag == "Enemy")
					{
						Debug.Log(i);
						enemies[i].gameObject.GetComponent<Enemy>().mHealth -= DotAoeDamage;
					}
				}
				DotAoeIntervalElapsed = 0;
			}
		}
		else
		{
			ResetStats();
			ObjectPool.Instance.destroy(gameObject);
		}
	}	
	
	void Explode_DoubleAoE()
	{
		LayerMask layerMask = LayerMask.NameToLayer("Enemy");
		layerMask = ~layerMask;
		enemies = Physics.OverlapSphere(transform.position, radius, layerMask);	
		
		foreach(Collider enemy in enemies)
		{
			Enemy tempEnemy = enemy.gameObject.GetComponent<Enemy>();
		  	if(enemy.tag == "Enemy")
			{
				tempEnemy.mHealth -= DoubleAoeDamage;
				tempEnemy.GetComponent<StatusEffect>().statusTypeAoe = StatusEffect.StatusType.DOUBLE_AOE;
			}
		}
		ResetStats();
		
		Instantiate(DoubleAoeExplosion, gameObject.transform.position, Quaternion.identity);
		
		ObjectPool.Instance.destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "HexGrid")
		{
			//Debug.Log("AAAAAAAAAAAAA");
			
			isStopMovement = true;
			
			if (gameObject.tag == "ProjectileNormalAoE")
			{   
				Explode_NormalAoE();
			}
			else if (gameObject.tag == "ProjectileAoEDis")
			{
				if(other.gameObject.tag == "Enemy")
				{
					StatusEffect tempEnemy = other.gameObject.GetComponentInChildren<StatusEffect>();
					
					if(tempEnemy.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_ONE || 
						tempEnemy.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_TWO ||
						tempEnemy.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_THREE)
					{
						tempEnemy.statusTypeDis = StatusEffect.StatusType.NONE;
					}
				}
				if(isFirstHit)
				{
					Explode_AoEDisabler();
				}
			}
			else if (gameObject.tag == "ProjectileDoTAoE")
			{
				Explode_DoTAoE();
			}
			else if (gameObject.tag == "ProjectileDoubleAoE")
			{
				Explode_DoubleAoE();	
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, radius);	
	}
	
	void ResetStats()
	{
		isStopMovement = false;
		isEnableUpdate = false;
		isFirstHit = true;
		mTarget = null;
		
		AoeDisablerDurationElapsed = 0;
		DotAoeDurationElapsed = 0;
		DotAoeIntervalElapsed = 0;
		
		GetComponent<Renderer>().enabled = true;		
	}
}
