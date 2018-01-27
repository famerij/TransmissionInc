using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 5.0f;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
