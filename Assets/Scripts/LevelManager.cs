using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[Header("Transmitters")]
	[SerializeField] private MessageTransmitter[] enabledTransmitters = new MessageTransmitter[0];
	[SerializeField] private MessageTransmitter[] disabledTransmitters = new MessageTransmitter[0];
	[Header("Ship fader&die")]
	[SerializeField] private float shipStartFadeDistance = 15.0f;
	[SerializeField] private float shipMaxDistance = 20.0f;

	public UnityEvent AllCollectiblesCollected = new UnityEvent();

	private Ship ship;
	private Vector3 shipStartPosition;
	private List<Collectible> collectibles = new List<Collectible>();
	private FaderInOut fader;

	protected void Start ()
	{
		foreach(MessageTransmitter t in enabledTransmitters)
		{
			if (t != null)
			{
				t.enabled = true;
			}
		}
		foreach (MessageTransmitter t in disabledTransmitters)
		{
			if (t != null)
			{
				t.enabled = false;
			}
		}

		ship = FindObjectOfType<Ship>();
		shipStartPosition = ship.transform.position;

		fader = FindObjectOfType<FaderInOut>();
		if(fader == null)
		{
			Debug.LogError("No FaderInOut in the scene!");
		}
	}

	protected void Update()
	{
		if (fader != null)
		{
			float d = (ship.transform.position - shipStartPosition).magnitude;
			if (d > shipStartFadeDistance)
			{
				float alpha = 1.0f - ((shipMaxDistance - d) / (shipMaxDistance - shipStartFadeDistance));
				fader.SetBlackAlpha(alpha);
				if(d > shipMaxDistance)
				{
					Die();
				}
			}
		}
	}

	public void RegisterCollectible(Collectible c)
	{
		collectibles.Add(c);
    }

	public void OnShipCollided()
	{
		Die();
    }

	public void OnCollectibleCollected(Collectible collectible)
	{
		collectibles.Remove(collectible);
		if (collectibles.Count == 0)
		{
			OnAllCollectiblesCollected();
		}

		if(collectible is FastGate)
		{
			OnFastGateReached(collectible as FastGate);
		}
	}

	private void OnAllCollectiblesCollected()
	{
		if (AllCollectiblesCollected != null)
		{
			AllCollectiblesCollected.Invoke();
		}
	}

	private void OnFastGateReached(FastGate fastGate)
	{
		Debug.Log("FastGate reached, loading scene: " + fastGate.DestinationSceneName);
		StartCoroutine(DelayedSceneLoad(fastGate.DelayBeforeJump, fastGate.DestinationSceneName));
	}

	private IEnumerator DelayedSceneLoad(float delay, string sceneName)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(sceneName);
	}

	private void Die()
	{
		Debug.Log("Player died! Restarting scene.");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
