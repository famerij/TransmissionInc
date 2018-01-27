﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueLevelController : MonoBehaviour
{
	public Ship ship;
	public MessageTransmitter transmitter;
	public FaderInOut levelEndFadeOut;

	public float tumbleTime = 2f;

	public void AllCollectablesCollected()
	{
		transmitter.enabled = false;
		ship.enabled = false;

		StartCoroutine(SpinAndEndLevel());
	}

	private IEnumerator SpinAndEndLevel()
	{
		Transform shipTransform = ship.transform;
		float xSpeed = 1.1f;

		float startTime = Time.time;
		bool isFading = false;

		while (true)
		{
			if (!isFading && Time.time > startTime + tumbleTime)
			{
				isFading = true;
				levelEndFadeOut.Fade();
			}
			
			xSpeed += 1.1f * Time.deltaTime;
			shipTransform.Rotate(0, 0, -Time.deltaTime * 250);
			shipTransform.position = new Vector3(shipTransform.position.x + xSpeed * Time.deltaTime,
				shipTransform.position.y + 1.0f * Time.deltaTime, shipTransform.position.z);
			yield return null;
		}
	}

	public void OnLevelEndFadeOutComplete()
	{
		SceneManager.LoadScene("1_Level1");
	}
}