using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FastGate : Collectible
{
	[SerializeField] private string destinationSceneName = "INSERT";
	[SerializeField] private float delayBeforeJump = 2.0f;
	[SerializeField] private Transform animationRoot;
	[SerializeField] private Transform spaceShip;
	[SerializeField] private Animation animation;

	public string DestinationSceneName {  get { return destinationSceneName; } }
	public float DelayBeforeJump { get { return delayBeforeJump; } }

	private Collider2D collider;

	void Start()
	{
		collider = GetComponent<Collider2D>();
	}
	
	[ContextMenu("Collect")]
	public override void Collect()
	{
		collider.enabled = false;
		StartCoroutine(MoveSpaceShipToAnimationRoot());
		Debug.Log("Fastgate collected!");
	}

	IEnumerator MoveSpaceShipToAnimationRoot()
	{
		Camera.main.GetComponent<CinemachineVirtualCamera>().enabled = false;
		Debug.Log("Starting animation");
		animation.Play();
		audioSource.PlayOneShot(soundClip);	
		
		float duration = 1f;
		float t = 0f;
		while (t < 1f)
		{
			t += Time.deltaTime / duration;
			spaceShip.transform.localPosition =
				Vector3.Lerp(spaceShip.transform.position, animationRoot.position, t);
			yield return null;
		}
		spaceShip.SetParent(animationRoot, true);
	}
}
