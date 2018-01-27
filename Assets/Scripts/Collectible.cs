using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	[SerializeField] private GameObject graphics;

	[Header("Audio")]
	[SerializeField] protected AudioClip soundClip;

	private LevelManager levelManager;
	protected AudioSource audioSource;

	protected void OnEnable()
	{
		levelManager = FindObjectOfType<LevelManager>();
		audioSource = GetComponent<AudioSource>();
		levelManager.RegisterCollectible(this);
	}

	public virtual void Collect()
	{
		Debug.Log("Collected: " + gameObject.name);
		//TODO: Fire off effects if any are made
		audioSource.PlayOneShot(soundClip);
		graphics.SetActive(false);
		StartCoroutine(DelayedDestroy(soundClip.length));
	}

	IEnumerator DelayedDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Ship ship = coll.gameObject.GetComponent<Ship>();
		if (ship != null)
		{
			LevelManager levelManager = FindObjectOfType<LevelManager>();
			if (levelManager != null)
			{
				levelManager.OnCollectibleCollected(this);
			}
		}

		Collect();
	}

}
