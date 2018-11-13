using UnityEngine;
using System.Collections;

public class BackgroundRotationScript : MonoBehaviour 
{
	
	Transform mTransform;
	
	public float rotateSpeed = 10.0f;
	
	// Use this for initialization
	void Start () 
	{
		mTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTransform.RotateAround (Vector3.zero, Vector3.left, rotateSpeed * Time.deltaTime);
	}
}
