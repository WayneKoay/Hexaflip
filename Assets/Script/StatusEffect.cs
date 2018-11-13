using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour 
{	
	public enum StatusType
	{
		NONE = 0,
		NORMAL_DOT,
		NORMAL_SLOW_ONE,
		NORMAL_SLOW_TWO,
		NORMAL_SLOW_THREE,
		STUN,
		DOUBLE_DOT,
		DISABLER_DOT,
		DOUBLE_AOE,
		TOTAL
	}
	public StatusType statusTypeDot = StatusType.NONE;
	public StatusType statusTypeDis = StatusType.NONE;
	public StatusType statusTypeAoe = StatusType.NONE;
	
	
	public float DotDamage;
	public float DotInterval;
	public float DotDuration;
	
	public float DotDurationElapsed = 0;
	public float DotIntervalElapsed = 0;
	
	public float SlowRate;
	public float SlowDuration;
	public float SlowStackLimit;
	public float Slow;
	public int SlowStack = 0;
	public float SlowDurationElapsed = 0;
	
	public float StunDamage;
	public float StunTime;
	public float StunInterval;
	public float StunMaxCount;
	public float StunCurrentCount;
	public float StunIntervalElapsed = 0;
	public float StunTimeElapsed = 0;
	public bool isStunned = false;
	public bool StunAnim = false;
	public GameObject stunPrefab = null;
	
	public float prevEnemyHP = 0;
	public float DoubleDotDamage;
	public float DoubleDotDamagePercentage;
	public bool GetEnemyHP = false;
	public float DoubleDotDuration;
	public float DoubleDotDurationElapsed = 0;
	
	public float DisablerDotDuration;
	public float DisablerDotDurationElapsed;
	public float DisablerDotInterval;
	public float DisablerDotIntervalElapsed;
	public float DisablerDotDamage;
	
	public float DoubleAoeDuration;
	public float DoubleAoeDurationElapsed;
	public float DoubleAoeDamage;
	public float DoubleAoeRadius;
	
	Enemy tempEnemy;
	
	Collider[] enemies;
	
	// Use this for initialization
	void Start () 
	{
		tempEnemy = this.gameObject.GetComponent<Enemy>();
		StunIntervalElapsed = StunInterval;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!tempEnemy.IsDead)
		{
			if(statusTypeDot != StatusType.NONE || statusTypeDis != StatusType.NONE || statusTypeAoe != StatusType.NONE )
			{
				if(statusTypeDot == StatusType.NORMAL_DOT)
				{
					NormalDot();
				}
				else if(statusTypeDis == StatusType.NORMAL_SLOW_ONE ||
						statusTypeDis == StatusType.NORMAL_SLOW_TWO ||
						statusTypeDis == StatusType.NORMAL_SLOW_THREE)
				{
					SlowDebuff();	
				}
				else if(statusTypeDis == StatusType.STUN)
				{
					StunDebuff();
				}
				else if(statusTypeDot == StatusType.DOUBLE_DOT)
				{
					DoubleDoTDebuff();
				}
				else if(statusTypeDot == StatusType.DISABLER_DOT)
				{
					DisablerDoTDebuff();	
				}
				else if(statusTypeAoe == StatusType.DOUBLE_AOE)
				{
					DoubleAoEDebuff();	
				}
			}
			
			if(isStunned)
			{
				if (!StunAnim)
				{
					this.gameObject.GetComponentInChildren<Animation>().Play("Stun");
					
					Vector3 pos = this.transform.position + new Vector3(0, this.GetComponent<Collider>().bounds.size.y * 1.1f, 0);
					//ObjectPool.Instance.instantiate(stunPrefab, pos, Quaternion.identity);
					
					StunAnim = true;
				}
				Stun();	
			}
			else
			{
				this.gameObject.GetComponentInChildren<Animation>().PlayQueued("Walk", QueueMode.CompleteOthers);
			}
		}	
	}
	
	public void NormalDot()
	{		
		DotDurationElapsed += Time.deltaTime;
		DotIntervalElapsed += Time.deltaTime;
		if (DotDuration > DotDurationElapsed)
		{
			if (DotIntervalElapsed > DotInterval)
			{
				DotIntervalElapsed = 0;
				tempEnemy.mHealth -= DotDamage;
			}
		}
		else
		{
			statusTypeDot = StatusType.NONE;	
		}
	}
	
	public void SlowDebuff()
	{		
		if (statusTypeDis == StatusType.NORMAL_SLOW_ONE)
		{
			SlowStack = 1;
		}
		else if (statusTypeDis == StatusType.NORMAL_SLOW_TWO)
		{
			SlowStack = 2;
		}
		else if (statusTypeDis == StatusType.NORMAL_SLOW_THREE)
		{
			SlowStack = 3;
		}
		
		Slow = SlowRate * SlowStack;
		
		tempEnemy.mSpeed = tempEnemy.OriginalSpeed * (1 - Slow);
		
		SlowDurationElapsed += Time.deltaTime;
		
		if(SlowDurationElapsed > SlowDuration)
		{
			SlowStack = 0;
			tempEnemy.mSpeed = tempEnemy.OriginalSpeed;
			statusTypeDis = StatusType.NONE;
		}
	}
	
	public void StunDebuff()
	{
		StunIntervalElapsed += Time.deltaTime;
		
		// Check if duration of stun has not ended
		if (0 < StunCurrentCount)
		{
			// Check if interval of stun is reached
			if (StunIntervalElapsed > StunInterval)
			{
				//Reset the interval timer
				StunIntervalElapsed = 0;
				//Deal stun damage to enenmy
				tempEnemy.mHealth -= StunDamage;
				
				isStunned = true;
				StunCurrentCount -= 1;
			}
		}
		else
		{
			statusTypeDis = StatusType.NONE;
			tempEnemy.mSpeed = tempEnemy.OriginalSpeed;			
		}
	}
	
	public void Stun()
	{		
		StunTimeElapsed += Time.deltaTime;
		
		if (StunTimeElapsed < StunTime)
		{
			tempEnemy.mSpeed = 0;
		}
		else
		{
			StunAnim = false;
			
			this.gameObject.GetComponentInChildren<Animation>().PlayQueued("Recover", QueueMode.PlayNow);
			
			//ObjectPool.Instance.Destroy(stunPrefab);
			
			isStunned = false;
			StunTimeElapsed = 0;
			tempEnemy.mSpeed = tempEnemy.OriginalSpeed;
		}
	}
	
	public void DoubleDoTDebuff()
	{
		DoubleDotDurationElapsed += Time.deltaTime;
		
		// Get enemy hp
		if (!GetEnemyHP)
		{
			prevEnemyHP = tempEnemy.mHealth;
			GetEnemyHP = true;
		}
		
		if (DoubleDotDurationElapsed > DoubleDotDuration)
		{
			float currEnemyHP = tempEnemy.mHealth;
			DoubleDotDamage = (prevEnemyHP - currEnemyHP)*DoubleDotDamagePercentage;
			tempEnemy.mHealth -= DoubleDotDamage;
			DoubleDotDurationElapsed = 0.0f;
			GetEnemyHP = false;
			statusTypeDot = StatusType.NONE;
		}
	}
	
	public void DisablerDoTDebuff()
	{
		DisablerDotDurationElapsed += Time.deltaTime;	
		DisablerDotIntervalElapsed += Time.deltaTime;
		
		if(DisablerDotDurationElapsed < DisablerDotDuration)
		{
			if (DisablerDotIntervalElapsed >  DisablerDotInterval)
			{
				DisablerDotIntervalElapsed = 0;
				tempEnemy.mHealth -= DisablerDotDamage * tempEnemy.mSpeed;
			}
		}
		else
		{
			statusTypeDot = StatusType.NONE;	
		}
	}
	
	public void DoubleAoEDebuff()
	{
		DoubleAoeDurationElapsed += Time.deltaTime;
		
		if (DoubleAoeDurationElapsed > DoubleAoeDuration)
		{
			statusTypeAoe = StatusType.NONE;	
		}
	}
	
	public void DoubleAoEExplode()
	{
		LayerMask layerMask = LayerMask.NameToLayer("Enemy");
		layerMask = ~layerMask;
		enemies = Physics.OverlapSphere(transform.position, DoubleAoeRadius, layerMask);
		
		foreach(Collider enemy in enemies)
		{
		  	if(enemy.tag == "Enemy")
			{
				enemy.gameObject.GetComponent<Enemy>().mHealth -= DoubleAoeDamage;
			}
		}	
	}
}