using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class FXTrail : FXBase
{
	TrailRenderer mTrail;
	float mTrailTime;

	protected override void Awake()
	{
		base.Awake();
		mTrail = GetComponent<TrailRenderer>();
		mTrailTime = mTrail.time;
	}

	IEnumerator resetTrail()
	{
		yield return 0; // wait for one frame.
		mTrail.time = mTrailTime;
		mLife = mTrailTime;
	}

	void OnEnable()
	{
		StartCoroutine(resetTrail());
	}

	void OnDisable()
	{
		mTrail.time = -1.0f;
	}

	protected override void Update()
	{
		if (mTarget && !mTarget.gameObject.activeSelf) mTarget = null;
		base.Update();
	}
}
