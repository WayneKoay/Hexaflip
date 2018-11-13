using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	private static bool sIsQuitting = false;
	private static InputManager sInstance;
	public static InputManager Instance
	{
		get
		{
			if (sInstance == null && !sIsQuitting)
			{
				GameObject obj = new GameObject("InputManager");	
				obj.AddComponent<InputManager>();
			}
			
			return sInstance;	
		}
	}
	
	void OnEnable()
	{
		if (sInstance == null)
		{
			sInstance = this;
			//DontDestroyOnLoad(gameObject);
			return;
		}
		
		// Self destruct if another input manager already exist
		ObjectPool.Instance.destroy(gameObject);
	}
	
	void OnApplicationQuit()
	{
		sIsQuitting = true;
	}
	
	// On Touch Down/Up event.
	public delegate void OnTouchEvent(int fingerID, bool down, Vector2 pos);
	public OnTouchEvent EvtOnTouch;
	
	// On Touch Drag event.
	public delegate void OnTouchDragEvent(int fingerID, Vector2 pos);
	public OnTouchDragEvent EvtOnTouchDrag;
	
#if (!UNITY_ANDROID && !UNITY_IPHONE)
	private bool mMouseIsDown = false;
	private Vector2 mPrevMousePos;
#endif
	
	void Update()
	{
#if (UNITY_ANDROID || UNITY_IPHONE)
		Touch[] touches = Input.touches;
		GameObject.Find ("BuildManager").GetComponent<BuildManager>().debugLog =  "" + Input.touchCount;
		foreach (Touch touch in touches)
		{
			TouchPhase phase = touch.phase;
			if (phase == TouchPhase.Began)
			{
				// Call on touch event
				if (EvtOnTouch != null) EvtOnTouch(touch.fingerId, true, touch.position);
			}
			else if (phase == TouchPhase.Ended ||
					 phase == TouchPhase.Canceled)
			{
				// Call on touch event
				if (EvtOnTouch != null) EvtOnTouch(touch.fingerId, false, touch.position);
			}
			else if (phase == TouchPhase.Moved)
			{
				// TODO: Call on touch drag event
				if (EvtOnTouch != null) EvtOnTouchDrag(touch.fingerId, touch.position);
			}
		}
#else
		if (mMouseIsDown)
		{
			Vector2 currMousePos = Input.mousePosition;
			if (currMousePos != mPrevMousePos)
			{
				mPrevMousePos = Input.mousePosition;
				
				// TODO: Call on touch drag event
				if (EvtOnTouch != null) EvtOnTouchDrag(0, currMousePos);
			}
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			mMouseIsDown = true;
			mPrevMousePos = Input.mousePosition;
			
			// Call on touch event
			if (EvtOnTouch != null) EvtOnTouch(0, true, mPrevMousePos);	
		}
		else if (Input.GetMouseButtonUp(0))
		{
			mMouseIsDown = false;
			
			// Call on touch event
			if (EvtOnTouch != null) EvtOnTouch(0, false, mPrevMousePos);	
		}
#endif
	}
}
