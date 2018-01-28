using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class ConsoleMessage : MonoBehaviour
{
	public TextAsset script;
	public bool showOnStart;
	public TMP_Text text;
	public float pausePerCharacter = 0.055f;
	public UnityEvent onDone;

	private int currentLine = 0;
	private string[] lines;
	private Coroutine showCharactersCoroutine;
	private AudioSource audioSource;

	protected void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		text.text = "";
		lines = script.text.Replace("\r", "").Split(new char[] {'\n'});

		if (LevelManager.deathLevel == SceneManager.GetActiveScene().name)
		{
			// User just died on this level, skip message
			onDone.Invoke();
			gameObject.SetActive(false);
		}
		else
		{
			ShowTextLine();
		}
	}

	public void Show()
	{
		ShowTextLine();
	}

	private void ShowTextLine()
	{
		showCharactersCoroutine = StartCoroutine(ShowLine());
	}

	public IEnumerator ShowLine()
	{
		string line = lines[currentLine];
		text.text = line;
		text.maxVisibleCharacters = 0;

		int totalCharacterCount = lines[currentLine].Length;
		while (text.maxVisibleCharacters < totalCharacterCount)
		{
			audioSource.pitch = Random.Range(0.5f, 1f);
			audioSource.Play();
			text.maxVisibleCharacters++;
			yield return new WaitForSeconds(pausePerCharacter);
		}
		showCharactersCoroutine = null;
	}

	protected void Update ()
	{
		if (Input.anyKeyDown) 
		{
			if (showCharactersCoroutine != null)
			{
				StopCoroutine(showCharactersCoroutine);
				showCharactersCoroutine = null;
				text.maxVisibleCharacters = lines[currentLine].Length;
			}
			else
			{
				currentLine++;
				if (currentLine < lines.Length)
				{
					ShowTextLine();
				}
				else
				{
					onDone.Invoke();
					gameObject.SetActive(false);
				}
			}
		}
	}
}
