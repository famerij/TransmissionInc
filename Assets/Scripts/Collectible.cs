using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	[SerializeField] private float collectionRange = 2.0f;

	[Header("Audio")]
	[SerializeField] private AudioClip soundClip;

	private LevelManager levelManager;
	private AudioSource audioSource;
	
	public float CollectionRange {  get { return collectionRange; } }

	protected void OnEnable()
	{
		levelManager = FindObjectOfType<LevelManager>();
		levelManager.AddCollectible(this);
		audioSource = GetComponent<AudioSource>();
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
		audioSource.PlayOneShot(soundClip);
	}

}
