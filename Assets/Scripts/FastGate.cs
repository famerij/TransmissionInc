using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastGate : Collectible
{
	[SerializeField] private string destinationSceneName = "INSERT";
	[SerializeField] private float delayBeforeJump = 2.0f;

	public string DestinationSceneName {  get { return destinationSceneName; } }
	public float DelayBeforeJump { get { return delayBeforeJump; } }

	public override void Collect()
	{
		Debug.Log("Fastgate collected!");
		audioSource.PlayOneShot(soundClip);
	}
}
