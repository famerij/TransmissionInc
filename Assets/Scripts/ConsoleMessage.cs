using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

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

	protected void Start ()
	{
		text.text = "";
		lines = script.text.Replace("\r", "").Split(new char[] {'\n'});
		
		ShowTextLine();
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
