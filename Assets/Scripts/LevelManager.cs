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

	public UnityEvent AllCollectiblesCollected = new UnityEvent();

	//private Ship ship;
	private List<Collectible> collectibles = new List<Collectible>();

	protected void Start ()
	{
		foreach(MessageTransmitter t in enabledTransmitters)
		{
			t.enabled = true;
		}
		foreach (MessageTransmitter t in disabledTransmitters)
		{
			t.enabled = false;
		}

		//ship = FindObjectOfType<Ship>();
	}

	public void RegisterCollectible(Collectible c)
	{
		collectibles.Add(c);
    }

	private void OnAllCollectiblesCollected()
	{
		//TODO: level done etc.
		Debug.Log("All collectibles collected");
		if(AllCollectiblesCollected != null)
		{
			AllCollectiblesCollected.Invoke();
        }
	}

	public void OnShipCollided()
	{
		//TODO: Restart level
		Debug.Log("Collision! TODO: Restart level");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnCollectibleCollected(Collectible collectible)
	{
		collectibles.Remove(collectible);
		if (collectibles.Count == 0)
		{
			OnAllCollectiblesCollected();
		}
	}


}
