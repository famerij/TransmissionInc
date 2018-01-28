using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelController : MonoBehaviour
{
	public Ship ship;
	public MessageTransmitter transmitter;
	public FaderInOut levelEndFadeOut;
	public string nextLevel;
	public LevelManager levelManager;

	public float tumbleTime = 2f;

	private Coroutine spinRoutine;
	private bool levelStarted;

	protected void Start()
	{
		ship.ToggleEnabled(true);
	}

	//protected void Update()
	//{
	//	if(!levelStarted && Input.anyKeyDown)
	//	{
	//		ship.ToggleEnabled(true);
	//	}
	//}

	public void AllCollectablesCollected()
	{
		transmitter.enabled = false;
		ship.enabled = false;
		levelManager.enabled = false;

		spinRoutine = StartCoroutine(SpinAndEndLevel());
	}

	private IEnumerator SpinAndEndLevel()
	{
		Transform shipTransform = ship.transform;
		float xSpeed = 1.1f;

		float startTime = Time.time;
		bool isFading = false;
		ship.ToggleEnabled(false);

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
				shipTransform.position.y - 0.5f * Time.deltaTime, shipTransform.position.z);
			yield return null;
		}
	}

	public void OnLevelEndFadeOutComplete()
	{
		StopCoroutine(spinRoutine);
		StartCoroutine(WaitThenEndLevel());
	}

	public IEnumerator WaitThenEndLevel()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(nextLevel);
	}
}
