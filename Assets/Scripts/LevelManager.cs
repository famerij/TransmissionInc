using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
	[Header("Transmitters")]
	[SerializeField] private MessageTransmitter[] enabledTransmitters = new MessageTransmitter[0];
	[SerializeField] private MessageTransmitter[] disabledTransmitters = new MessageTransmitter[0];

	public UnityEvent AllCollectiblesCollected = new UnityEvent();

	private Ship ship;
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

		ship = FindObjectOfType<Ship>();
	}

	public void AddCollectible(Collectible c)
	{
		collectibles.Add(c);
    }

	public void OnCollectCollectible(Collectible c)
	{
		collectibles.Remove(c);
		if(collectibles.Count == 0)
		{
			OnAllCollectiblesCollected();
        }
    }

	protected void Update()
	{
		for(int i = collectibles.Count -1; i >= 0; i--)
		{
			if (Utils.InRange(collectibles[i].transform.position, ship.transform.position, collectibles[i].CollectionRange))
			{
				collectibles[i].Collect();
				collectibles.Remove(collectibles[i]);
				if (collectibles.Count == 0)
				{
					OnAllCollectiblesCollected();
				}
			}
		}
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

}
