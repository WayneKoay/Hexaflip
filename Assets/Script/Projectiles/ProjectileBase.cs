using UnityEngine;
using System.Collections;

public class ProjectileBase : MonoBehaviour
{
	public float Speed;		
	public float Range;
	private float Distance;

	public float Damage;

	public GameObject FXPrefab;
	public GameObject TargetEnemy = null;

	protected Transform mTransform;

	void Awake()
	{
		mTransform = transform;
	}

	void OnEnable()
	{
		Distance = 0.0f;

		GameObject fxObj = ObjectPool.Instance.instantiate(FXPrefab);
		fxObj.GetComponent<FXBase>().setTarget(mTransform);
	}

	protected void move()
	{
		float travelDistance = Time.deltaTime * Speed;
		mTransform.Translate(Vector3.forward * travelDistance);
		Distance += travelDistance;

		if (Distance > Range)
		{
			ObjectPool.Instance.destroy(gameObject);
		}
	}

	public virtual void aimAt(Transform target)
	{
		mTransform.LookAt(target.position);
	}

	// Update is called once per frame
	protected virtual void Update() 
	{
		move();
		
		//mTransform.position = TargetEnemy.transform.position;
	}
}
