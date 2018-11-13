using UnityEngine;
using System.Collections;

public class TokenScript : MonoBehaviour {
	/*
	public float fadeTime = 5.0f;
	private float fadeTimer;
	*/
	public float rotateSpeed;
	
	private TouchControl touchControl;
	private PlayerManager playerManager;
	
	private float fadeTimer;
	public float fadeTime;
	
	private float fadeOutTimer;
	public float fadeOutTime;
	
	Color colorStart;
	Color colorEnd;
	
	void OnDisable()
	{
		touchControl.selectedGameObject = null;
		fadeTimer = 0.0f;	
		fadeOutTimer = 0.0f;
		GetComponent<Renderer>().material.color = colorStart; 
	}
	
	// Use this for initialization
	void Start () 
	{
		touchControl = GameObject.Find("PlayerManager").GetComponent<TouchControl>();
		playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		
		colorStart = GetComponent<Renderer>().material.color;
 		colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(touchControl.selectedGameObject == gameObject)
		{
			playerManager.Token += 1;	
			ObjectPool.Instance.destroy(gameObject);
		}
		
		fadeTimer += Time.deltaTime;
		
		if(fadeTimer > fadeTime)
		{
		    FadeOut();
			
			if(GetComponent<Renderer>().material.color.a <= 0.0f)
			{
				ObjectPool.Instance.destroy(gameObject);
			}
		}
		transform.RotateAround (transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
		
		Blinking();
	}
	
	void FadeOut()
	{
		fadeOutTimer += Time.deltaTime;
		GetComponent<Renderer>().material.color = Color.Lerp (colorStart, colorEnd, fadeOutTimer/fadeOutTime);
	}
	
	void Blinking()
	{
		if(Time.time % 2 > 1)
		{
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 0.2f);
		}
		else
		{
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1.0f);
		}	
	}
}
