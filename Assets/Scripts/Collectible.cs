using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	[SerializeField] private float collectionRange = 2.0f;

	private LevelManager levelManager;
	public float CollectionRange {  get { return collectionRange; } }

	protected void OnEnable()
	{
		levelManager = FindObjectOfType<LevelManager>();
		levelManager.AddCollectible(this);
	}

	protected void Update()
	{
		Utils.DrawCircle(transform.position, collectionRange, Color.red);
	}


	public void Collect()
	{
		Debug.Log("Collected!");
		Destroy(gameObject);
		//TODO: Fire off effects, sounds etc.
	}

}
