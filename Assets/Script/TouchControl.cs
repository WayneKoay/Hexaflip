using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour 
{
	public bool mShowDebugLine = true;
	public Color mLineColor = Color.white;
	private LineRenderer mDebugLine;
	
	public Rect mRegion;	
	public float mRadius;
	private int mFingerID = -1;
	private Vector2 mAnchorPos;
	private Vector2 mPos;	
	
	public Rect[] mIgnoreRegionList;
	
	Ray ray;
	
	GameObject tempTower;
	
	public GameObject selectedGameObject;
	public GameObject tempTile;
	
	private GridManager gridManager;
	private BuildManager buildManager;
	private PlayerManager playerManager;
	private TileManager tileManager;
	private TowerManager towerManager;
	private IngameUIScript ingameUI;
	
	private Tile selectedTileGC;
	private Tile tempTileGC;
	
	public void RaycastToTileLayer()
	{
		selectedGameObject = null;
		ray = Camera.main.ScreenPointToRay(new Vector3(mPos.x, mPos.y, 0.0f));
		
		RaycastHit hitInfo = new RaycastHit();
		if(Physics.Raycast(ray, out hitInfo, 10000.0f) && !buildManager.overGUIElement && !ingameUI.pause)
		{
			selectedGameObject = hitInfo.collider.gameObject;
		}
	}
	
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
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
		towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
		ingameUI = GameObject.Find("IngameUI").GetComponent<IngameUIScript>();
		
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
			}
			return;
		}
		
		// Skip if finger is releasing and we are not tracking anything yet
		if (!down) return;
		
	 	Vector2 viewportPos = Camera.main.ScreenToViewportPoint(pos);
		if (!mRegion.Contains(viewportPos)) return;
		
		for(int i=0; i<mIgnoreRegionList.GetLength(0); i++)
		{
			if (mIgnoreRegionList[i].Contains(viewportPos)) return;
		}
		
		mFingerID = fingerID;
		mAnchorPos = pos;
		mPos = pos;
		
		RaycastToTileLayer();
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
