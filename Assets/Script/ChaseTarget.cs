using UnityEngine;
using System.Collections;

public class ChaseTarget : MonoBehaviour 
{
	public Transform mTarget;
	public float mSpring = 10.0f;
	public float mDamper = 15.0f;
	
	//public float mSmoothing = 0.5f;
	Vector3 mVelocity = Vector3.zero;
	Vector3 mCamToTargetOffset;
	Transform mTransform;
	
	void Awake() 
	{
		mTransform = transform;
		mCamToTargetOffset = mTransform.position - mTarget.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currPos = mTransform.position;
		Vector3 offset = mTarget.position - currPos;
		offset += mCamToTargetOffset;
		
		Vector3 force = offset * mSpring;
		force -= mVelocity * mDamper;
		mVelocity += force * Time.deltaTime;
		
		mTransform.position = mTransform.position + mVelocity * Time.deltaTime;
	}
}
