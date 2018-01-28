using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	public bool isEnabled = true;
	
	[SerializeField] private float spaceshipSpeed = 1.0f;
	[SerializeField] private float spaceshipRotationSpeed = 1.0f;
	[SerializeField] private ParticleSystem mainThruster;
	[SerializeField] private ParticleSystem leftThruster;
	[SerializeField] private ParticleSystem rightThruster;

	[Header("Audio")]
	[SerializeField] private AudioClip collisionSound;
	[SerializeField] private AudioSource collisionAudioSource;
	[SerializeField] private AudioSource engineAudioSource;
	[Header("Effects")]
	[SerializeField] private GameObject deathEffectPrefab;

	private float rotationRate;
	public float RotationRate
	{
		get { return rotationRate; }
		set
		{
			if(value == 0.0f)
			{
				// Stop (counter) previous rotation:
				if(rotationRate < 0.0f)
				{
					rightThruster.Play();
				}
				else if (rotationRate > 0.0f)
				{
					leftThruster.Play();
				}
			}

			// Start new rotation:
			if(value < 0.0f)
			{
				leftThruster.Play();
			}
			else if(value > 0.0f)
			{
				rightThruster.Play();
			}
			rotationRate = value;
		}
	}

	void Start()
	{
		ToggleEnabled(isEnabled);
	}

	// Update is called once per frame
	void Update()
	{
		if (!isEnabled) return;
		
		this.transform.Rotate(Vector3.forward, RotationRate * spaceshipRotationSpeed * Time.deltaTime);
		this.transform.position += (transform.up * spaceshipSpeed * Time.deltaTime);
	}
	
	public void ToggleEnabled(bool enabled)
	{
		isEnabled = enabled;
		engineAudioSource.enabled = enabled;
		if (enabled)
		{
			engineAudioSource.Play();
			mainThruster.Play();
		}
		else
		{
			mainThruster.Stop();
		}
	}

	public void Collision()
	{
		ToggleEnabled(false);
		collisionAudioSource.PlayOneShot(collisionSound);
		if (deathEffectPrefab != null)
		{
			Instantiate<GameObject>(deathEffectPrefab, transform);
		}
		gameObject.AddComponent<Spinner>().RotationSpeed = -30.0f;
	}
}
