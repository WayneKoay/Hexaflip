using UnityEngine;
using System.Collections;

public class MovePan : MonoBehaviour 
{
	public float mMaxSpeed = 10.0f;
	public AnalogStick mMovementStick;
	
	private CharacterController mController;
	
	void Awake()
	{
		mController = GetComponent<CharacterController>();	
	}
	
	void Update()
	{
		Vector2 dir;
		float mag;
		
		mMovementStick.getDirection(out dir, out mag);
		
		Vector3 moveVel = new Vector3(dir.x, 0.0f, dir.y);
		moveVel *= mag * mMaxSpeed;
		mController.Move(moveVel);
	}
}
