using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	[SerializeField] private float spaceshipSpeed = 1.0f;
	[SerializeField] private float spaceshipRotationSpeed = 1.0f;
	
	[Header("Audio")]
	[SerializeField] private AudioClip collisionSound;

	private float rotationRate;
	public float RotationRate
	{
		get { return rotationRate; }
		set
		{
			rotationRate = value;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.Rotate(Vector3.forward, RotationRate * spaceshipRotationSpeed * Time.deltaTime);
		this.transform.position += (transform.up * spaceshipSpeed * Time.deltaTime);
	}
}
