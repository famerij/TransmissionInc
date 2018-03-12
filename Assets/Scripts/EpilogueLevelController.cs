﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EpilogueLevelController : MonoBehaviour
{
	public CanvasGroup missionReportCanvasGroup;

	public TMP_Text[] texts;
	public float fadeTime = 5f;
	public float pauseAfterFade = 1f;
	public float pausePerCharacter = 0.04f;
	public float pausePerLine = 1.75f;
	public float pauseLastLine = 3f;

	public AudioClip typeSound;
	public Image postMortem;

	private AudioSource audioSource;
	private bool reportDone = false;

	public void Start()
	{
		postMortem.enabled = false;
		audioSource = GetComponent<AudioSource>();
		missionReportCanvasGroup.gameObject.SetActive(false);
	}

	public void ConsoleDone()
	{
		StartCoroutine(DoMissionReport());
	}

	private IEnumerator DoMissionReport()
	{
		yield return new WaitForSeconds(1.3f);

		missionReportCanvasGroup.gameObject.SetActive(true);
		missionReportCanvasGroup.alpha = 0;
		foreach (TMP_Text text in texts)
		{
			text.maxVisibleCharacters = 0;
		}

		float startTime = Time.time;
		float endTime = Time.time + fadeTime;

		while (Time.time < endTime)
		{
			float t = (Time.time - startTime) / (endTime - startTime);
			missionReportCanvasGroup.alpha = t;
			yield return null;
		}

		yield return new WaitForSeconds(pauseAfterFade);

		StartCoroutine(DelayedShowPostMortem());

		missionReportCanvasGroup.alpha = 1;

		for (int i = 0; i < texts.Length; i++)
		{
			TMP_Text text = texts[i];

			int charCount = text.text.Length;
			for (int j = 0; j <= charCount; j++)
			{
				text.maxVisibleCharacters = j;

				audioSource.pitch = Random.Range(.9f, 1.1f);
				audioSource.PlayOneShot(typeSound);
				yield return new WaitForSeconds(pausePerCharacter);
			}
			yield return new WaitForSeconds(i == texts.Length - 2 ? pauseLastLine : pausePerLine);
		}

		reportDone = true;
	}

	private IEnumerator DelayedShowPostMortem()
	{
		yield return new WaitForSeconds(11f);
		postMortem.enabled = true;

		float startTime = Time.time;
		float endTime = Time.time + fadeTime;

		while (Time.time < endTime)
		{
			Color color = postMortem.color;

			float targetAlpha = (Time.time - startTime) / (endTime - startTime);
			color.a = targetAlpha;
			postMortem.color = color;
			yield return null;
		}
	}

	void Update()
	{
		if (reportDone && Input.anyKeyDown)
		{
			Debug.Log("Quitting game.");
			Application.Quit();
		}
	}
}
