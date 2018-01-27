using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueLevelController : MonoBehaviour
{
	public Ship ship;
	public MessageTransmitter transmitter;
	public FaderInOut levelEndFadeOut;
	public string nextLevel;

	public GameObject[] disableOnStartObjects;

	public void Start()
	{
		transmitter.enabled = false;
		ship.enabled = false;

		foreach (var go in disableOnStartObjects)
		{
			go.SetActive(false);
		}

		StartCoroutine(Spin());
	}

	private IEnumerator Spin()
	{
		Transform shipTransform = ship.transform;
		float xSpeed = 0.3f;

		while (true)
		{
			// Slow down until stop
			xSpeed *= 0.999f;

			shipTransform.Rotate(0, 0, -Time.deltaTime * 250);
			shipTransform.position = new Vector3(shipTransform.position.x + xSpeed * Time.deltaTime,
				shipTransform.position.y, shipTransform.position.z);
			yield return null;
		}
	}

	public void OnLevelEndFadeOutComplete()
	{
		StartCoroutine(WaitThenEndLevel());
	}

	public IEnumerator WaitThenEndLevel()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(nextLevel);
	}
}
