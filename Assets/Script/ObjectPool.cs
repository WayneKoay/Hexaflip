using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
	
	//Singleton
	public static ObjectPool Instance {get;private set;}
	
	Dictionary<string, List<GameObject>> mPool = new Dictionary<string, List<GameObject>>();
	
	void Awake()
	{
		if(Instance != null)
		{
			ObjectPool.Instance.destroy(gameObject);
			return;
		}
		Instance = this;	
	}
	
	public GameObject instantiate(GameObject prefab)
	{
		if (mPool.ContainsKey(prefab.name))
		{
			List<GameObject> prefabPool = mPool[prefab.name];
			if(prefabPool.Count > 0)
			{
				int lastIndex = prefabPool.Count - 1;
				GameObject obj = prefabPool[lastIndex];
				prefabPool.RemoveAt(lastIndex);
				
				obj.transform.parent = null;
				obj.SetActive(true);
				return obj;
			}
			
			// Nothing in the pool, return a new one
			GameObject newObj = (GameObject)Instantiate(prefab);
			newObj.name = prefab.name;
			return newObj;
		}
		
		// If no such prefab has been created before,
		// Add prefab and instantiate
		mPool[prefab.name] = new List<GameObject>();
		GameObject newObject = (GameObject)Instantiate(prefab);
		newObject.name = prefab.name;
		return newObject;
	}
	
	public GameObject instantiate(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		GameObject obj = instantiate(prefab);
		Transform objTrans = obj.transform;
		objTrans.position = pos;
		objTrans.rotation = rot;
		return obj;
	}
	/// <summary>
	/// ObjectPool.Instance.destroy the specified obj.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	public void destroy(GameObject obj)
	{
		obj.SetActive(false);
		obj.transform.parent = transform;
		obj.transform.position = transform.position;
		
		mPool[obj.name].Add(obj);
	}
	
}
