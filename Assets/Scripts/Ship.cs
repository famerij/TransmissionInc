using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	[SerializeField] private float spaceshipSpeed = 1.0f;
	[SerializeField] private float spaceshipRotationSpeed = 1.0f;
	[SerializeField] private ParticleSystem leftThruster;
	[SerializeField] private ParticleSystem rightThruster;

	[Header("Audio")]
	[SerializeField] private AudioClip collisionSound;


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

	// Update is called once per frame
	void Update()
	{
		this.transform.Rotate(Vector3.forward, RotationRate * spaceshipRotationSpeed * Time.deltaTime);
		this.transform.position += (transform.up * spaceshipSpeed * Time.deltaTime);
	}
}
