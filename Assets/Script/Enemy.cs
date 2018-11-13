using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour 
{
	public float MaxHealth;
	public float mHealth;
	public int mPathIndex = 0;
	public float mSpeed = 1.0f;
	public float OriginalSpeed;
	public float mTurnSpeed = 10.0f;
	public bool mForceGoToNext = false;
	public Vector3 mForcePosition;
	
	public bool HasToken = false;
	public GameObject TokenIndicator = null;
	public GameObject tempTokenIndicator;
	public GameObject Token = null;
	 
	public bool IsDead = false;
	public bool DeathAnim = false;
	
	Color colorStart;
	Color colorEnd;
	public float fadeOutTimer = 0.0f;
	public float fadeOutDuration = 3.0f;
	
	private GridManager gridManager;
	private BuildManager buildManager;
	private StatusEffect statusEffect;	
	private PlayerManager playerManager;
	
	public List<PathNode> solvedPath = new List<PathNode>();
	
	SkinnedMeshRenderer skinnedMeshRenderer;
	
	public Texture healthBarTexture;
	public Texture healthBarFrameTexture;
	
	public GameObject DoubleDotExplosion = null;
	
	public static int enemyCount = 0;
	
	void Awake()
	{
		mSpeed = OriginalSpeed;
		mHealth = MaxHealth;
		IsDead = false;
	}
	
	void OnEnable()
	{
		mSpeed = OriginalSpeed;
		mHealth = MaxHealth;
		IsDead = false;
		DeathAnim = false;
		fadeOutTimer = 0.0f;
		enemyCount++;
	}
	
	public void AddToken()
	{
		HasToken = true;
		
		skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
		tempTokenIndicator = (GameObject)Instantiate(TokenIndicator, transform.position + new Vector3(0, skinnedMeshRenderer.bounds.size.y, 0), Quaternion.identity);
	}
	
	void OnDisable()
	{
		mPathIndex = 0;	
		skinnedMeshRenderer.material.color = colorStart;
		enemyCount--;
		HasToken = false;
		statusEffect.statusTypeAoe = StatusEffect.StatusType.NONE;
		statusEffect.statusTypeDis = StatusEffect.StatusType.NONE;
		statusEffect.statusTypeDot = StatusEffect.StatusType.NONE;
	}
	
	void Start () 
	{
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		
		transform.LookAt(solvedPath[0].transform.position);
		statusEffect = this.gameObject.GetComponent<StatusEffect>();
		
		skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
		
		colorStart = skinnedMeshRenderer.material.color;
		
 		colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0.0f);
	}
		
	// Update is called once per frame
	void Update () 
	{
		if(mForceGoToNext)
		{
			Vector3 forceDistance = mForcePosition - transform.position;
			forceDistance.y = 0.0f;
			if(forceDistance.sqrMagnitude < 0.001f * mSpeed * mSpeed)
			{
				mForceGoToNext = false;
				
				if(mPathIndex < solvedPath.Count)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(solvedPath[mPathIndex].transform.position - transform.position), Time.deltaTime * mTurnSpeed);
				}
			}
			else
			{
				transform.Translate(forceDistance.normalized * mSpeed * Time.deltaTime, Space.World);
			}
			
			return;
		}
		
		//Move the enemy along the path
		Vector3 distance = solvedPath[mPathIndex].Position - transform.position;
		distance.y = 0.0f;
		if(distance.sqrMagnitude < 0.001f * mSpeed * mSpeed)
		{
			mPathIndex++;
			//Kill the enemy if they reach the end
			if(mPathIndex >= solvedPath.Count)
			{
				playerManager.Health -= 1;
        		ObjectPool.Instance.destroy(gameObject);
				if(tempTokenIndicator != null)
				{
					DestroyImmediate(tempTokenIndicator);	
					tempTokenIndicator = null;
				}
			}
			//Force enemy to go to next node incase there is a recalculation due to building towers
			if(mPathIndex < solvedPath.Count)
			{
				mForcePosition = solvedPath[mPathIndex].transform.position;
			}
		}
		else
		{
			transform.Translate(distance.normalized * mSpeed * Time.deltaTime, Space.World);
		}
		
		//Enemy rotates to next node if haven't reach the end
		if(mPathIndex < solvedPath.Count)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(solvedPath[mPathIndex].transform.position - transform.position), Time.deltaTime * mTurnSpeed);
		}
		
		if (mHealth <= 0)	
		{
			IsDead = true;
			mHealth = 0;
			Dead();
			if(tempTokenIndicator != null)
			{
				Destroy(tempTokenIndicator);	
				tempTokenIndicator = null;
			}
		}
		
		if(tempTokenIndicator != null)
		{
			tempTokenIndicator.transform.position = transform.position + new Vector3(0, skinnedMeshRenderer.bounds.size.y, 0);
		}
	}
	
	void Dead()
	{
		DropToken();
		IsDead = true;
		if(statusEffect.statusTypeAoe == StatusEffect.StatusType.DOUBLE_AOE)
		{
			Instantiate(DoubleDotExplosion, gameObject.transform.position, Quaternion.identity);
			
			this.gameObject.GetComponent<StatusEffect>().DoubleAoEExplode();
    		ObjectPool.Instance.destroy(gameObject);
		}
		else
		{
			if(!DeathAnim)
			{
				mSpeed = 0;
				this.gameObject.GetComponentInChildren<Animation>().Play("Death");
				DeathAnim = true;
			}			
			if (!this.gameObject.GetComponentInChildren<Animation>().IsPlaying("Death"))
			{
				FadeOut();
				if(skinnedMeshRenderer.material.color.a <= 0.0f)
				{
					ObjectPool.Instance.destroy(gameObject);
				}
			}
			
		}
	}
	
	public void DropToken()
	{
		if(HasToken)
		{
			ObjectPool.Instance.instantiate(Token, transform.position, Quaternion.identity);
			HasToken = false;
		}
	}
	
	void FadeOut()
	{		
		fadeOutTimer += Time.deltaTime;
	    skinnedMeshRenderer.material.color = Color.Lerp (colorStart, colorEnd, fadeOutTimer/fadeOutDuration);
	}

	public void OnGUI()
	{
		if(healthBarTexture != null)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.0f, 30f, 0.0f));
			
			if(mHealth > 0)
			{
				GUI.DrawTexture(new Rect(screenPos.x - healthBarFrameTexture.width/2, Screen.height - screenPos.y, healthBarFrameTexture.width, healthBarFrameTexture.height), healthBarFrameTexture);			
				
				GUI.DrawTexture(new Rect(screenPos.x - healthBarTexture.width/2 + (healthBarTexture.width * (1.0f - mHealth/(float)MaxHealth)/2.0f), 2 + Screen.height - screenPos.y, healthBarTexture.width * mHealth/(float)MaxHealth, healthBarTexture.height), healthBarTexture);
			}
		}
	}
}
