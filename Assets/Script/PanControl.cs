using UnityEngine;
using System.Collections;

public class PanControl : MonoBehaviour 
{
	public bool mShowDebugLine = true;
	public Color mLineColor = Color.white;
	private LineRenderer mDebugLine;
	
	public Rect mRegion;	
	public float mRadius;
	private int mFingerID = -1;
	private Vector2 mAnchorPos;
	private Vector2 mPos;	
	
	void OnEnable() 
	{
		InputManager inputManager = InputManager.Instance;
		inputManager.EvtOnTouch += OnTouch;
		inputManager.EvtOnTouchDrag += OnTouchDrag;
	}
	
	void OnDisable()
	{
		InputManager inputManager = InputManager.Instance;
		if (inputManager != null)
		{
			inputManager.EvtOnTouch -= OnTouch;
			inputManager.EvtOnTouchDrag -= OnTouchDrag;		
		}
	}
	
	void Awake()
	{
		if (mShowDebugLine)
		{
			GameObject go = new GameObject("DebugLine");
			go.transform.parent = transform;
			mDebugLine = go.AddComponent<LineRenderer>();
			mDebugLine.material = new Material(Shader.Find("Mobile/Particles/Additive"));
			mDebugLine.SetColors(mLineColor, mLineColor);
			mDebugLine.SetVertexCount(2);
			mDebugLine.SetWidth(0.01f, 0.01f);
			mDebugLine.enabled = false;
		}
	}
	
	void OnTouch(int fingerID, bool down, Vector2 pos)
	{
		// If already tracking finger
		if (mFingerID != -1) 
		{
			// If tracking finger got lifted up, stop tracking finger id.
			if (mFingerID == fingerID && !down)
			{
				mFingerID =-1;
				mDebugLine.enabled = false;
			}
			return;
		}
		
		// Skip if finger is releasing and we are not tracking anything yet
		if (!down) return;
		
		Vector2 viewportPos = Camera.main.ScreenToViewportPoint(pos);
		if (!mRegion.Contains(viewportPos)) return;
		
		mFingerID = fingerID;
		mAnchorPos = pos;
		mPos = pos;
	}
	
	void OnTouchDrag(int fingerID, Vector2 pos)
	{
		if (mFingerID != fingerID) return;
		mPos = pos;
	}
	
	void updateDebugLine()
	{
		Camera cam = Camera.main;
		Vector3 screenAnchorPos = new Vector3(mAnchorPos.x, mAnchorPos.y, 1.0f);
		Vector3 screenPos = new Vector3(mPos.x, mPos.y, 1.0f);
		
		Vector3 anchorPos = cam.ScreenToWorldPoint(screenAnchorPos);
		Vector3 pos = cam.ScreenToWorldPoint(screenPos);
		mDebugLine.SetPosition(0, anchorPos);
		mDebugLine.SetPosition(1, pos);
	}
	
	void OnDrawGizmos()
	{
		int segments = 32;
		float theta = 0.0f;
		float deltaTheta = Mathf.PI * 2.0f / segments;

		Camera cam = Camera.main;
		Vector3 startPos = new Vector3(mAnchorPos.x + mRadius, mAnchorPos.y, 1.0f);
		Vector3 prevWorldPos = cam.ScreenToWorldPoint(startPos);
		
		for (int i = 0; i <= segments; ++i)
		{
			float x = Mathf.Cos(theta) * mRadius; 
			float y = Mathf.Sin(theta) * mRadius; 
			
			Vector3 screenPos = new Vector3(mAnchorPos.x + x, mAnchorPos.y + y, 1.0f);
			Vector3 currWorldPos = cam.ScreenToWorldPoint(screenPos);
			Gizmos.DrawLine(prevWorldPos, currWorldPos);
			prevWorldPos = currWorldPos;
			
			theta += deltaTheta;
		}
	}
}
