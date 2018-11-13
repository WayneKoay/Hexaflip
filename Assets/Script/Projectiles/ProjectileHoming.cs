using UnityEngine;
using System.Collections;

public class ProjectileHoming : ProjectileBase
{
	public Transform mTarget;

	public override void aimAt(Transform target)
	{
		mTarget = target;
	}

	// Update is called once per frame
	protected override void Update() 
	{
		if (mTarget && !mTarget.gameObject.activeSelf) mTarget = null;
		else if (mTarget) mTransform.LookAt(mTarget.position);
		
		move();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			Enemy tempEnemy = other.gameObject.GetComponent<Enemy>();
			StatusEffect statusEffect = tempEnemy.GetComponent<StatusEffect>();
			if (gameObject.tag == "ProjectileNormalDoT")
			{   
				tempEnemy.mHealth -= Damage;
				
				if(statusEffect.statusTypeDot == StatusEffect.StatusType.DOUBLE_DOT)
				{
					return;	
				}
				
				statusEffect.DotDurationElapsed = 0;
				
				statusEffect.statusTypeDot = StatusEffect.StatusType.NORMAL_DOT;
				
				ObjectPool.Instance.destroy(gameObject);
			}
			else if (gameObject.tag == "ProjectileNormalDis")
			{ 
				tempEnemy.mHealth -= Damage;
				
				statusEffect.SlowDurationElapsed = 0;
				
				if (statusEffect.statusTypeDis == StatusEffect.StatusType.NONE)
				{
					statusEffect.statusTypeDis = StatusEffect.StatusType.NORMAL_SLOW_ONE;
				}
				else if (statusEffect.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_ONE)
				{
					statusEffect.statusTypeDis = StatusEffect.StatusType.NORMAL_SLOW_TWO;
				}
				else if (statusEffect.statusTypeDis == StatusEffect.StatusType.NORMAL_SLOW_TWO)
				{
					statusEffect.statusTypeDis = StatusEffect.StatusType.NORMAL_SLOW_THREE;
				}
		
				ObjectPool.Instance.destroy(gameObject);
			}
			else if (gameObject.tag == "ProjectileDoubleDis")
			{ 
				tempEnemy.mHealth -= Damage;
				statusEffect.StunCurrentCount = statusEffect.StunMaxCount;
				
				statusEffect.statusTypeDis = StatusEffect.StatusType.STUN;
				
				ObjectPool.Instance.destroy(gameObject);
			}
			else if (gameObject.tag == "ProjectileDoubleDoT")
			{ 
				tempEnemy.mHealth -= Damage;
				
				statusEffect.statusTypeDot = StatusEffect.StatusType.DOUBLE_DOT;
				
				ObjectPool.Instance.destroy(gameObject);
			}
			else if (gameObject.tag == "ProjectileDisablerDoT")
			{ 
				tempEnemy.mHealth -= Damage;
				statusEffect.DisablerDotDurationElapsed = 0;
				
				statusEffect.statusTypeDot = StatusEffect.StatusType.DISABLER_DOT;
				
				ObjectPool.Instance.destroy(gameObject);
			}
		}
	}
}
