using UnityEngine;
using System.Collections;

public class FXBase : MonoBehaviour
{
	protected Transform mTransform;
	protected Transform mTarget;
	protected float mLife;

	protected virtual void Awake()
	{
		mTransform = transform;
	}

	public void setTarget(Transform target)
	{
		mTarget = target;
		mTransform.position = target.position;
	}

	protected virtual void Update()
	{
		if (mTarget == null)
		{
			// self destruct once life run out.
			mLife -= Time.deltaTime;
			if (mLife <= 0.0f)
			{
				ObjectPool.Instance.destroy(gameObject);
			}

			return;
		}

		mTransform.position = mTarget.position;
	}
}
