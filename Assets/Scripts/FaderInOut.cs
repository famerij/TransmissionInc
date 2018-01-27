using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FaderInOut : MonoBehaviour
{
	public Image black;
	public FadeType fadeType;
	public float startDelay = 0;
	public float fadeTime = 1.0f;
	public bool fadeOnStart;

	public UnityEvent OnDone;

	protected void Start()
	{
		if (fadeOnStart)
		{
			Fade();
		}
	}

	public void Fade()
	{
		StartCoroutine(DoFade());
	}

	private IEnumerator DoFade()
	{
		float fromAlpha = fadeType == FadeType.FadeIn ? 1 : 0;
		float toAlpha = fadeType == FadeType.FadeIn ? 0 : 1;

		if (startDelay > 0)
		{
			SetBlackAlpha(fromAlpha);
			yield return new WaitForSeconds(startDelay);
		}

		float startTime = Time.time;
		float endTime = startTime + fadeTime;

		while (Time.time < endTime)
		{
			float targetScale = (Time.time - startTime) / (endTime - startTime);
			float targetAlpha = Mathf.Lerp(fromAlpha, toAlpha, targetScale);
			SetBlackAlpha(targetAlpha);
			yield return 0;
		}

		OnDone.Invoke();
	}

	public void SetBlackAlpha(float alpha)
	{
		Color curColor = black.color;
		curColor.a = alpha;
		black.color = curColor;
	}

	public enum FadeType
	{
		FadeIn,
		FadeOut
	}
}
