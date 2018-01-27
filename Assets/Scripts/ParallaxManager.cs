using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
	[SerializeField] private Transform Far;
	[SerializeField] private float FarSpeed;
	[SerializeField] private Transform Near;
	[SerializeField] private float NearSpeed;
	[SerializeField] private Transform Nearest;
	[SerializeField] private float NearestSpeed;

	private Vector3 previousPosition;
	
	void Update()
	{
		var delta = transform.position - previousPosition;

		Far.transform.position -= delta * FarSpeed * Time.deltaTime;
		Near.transform.position -= delta * NearSpeed * Time.deltaTime;
		Nearest.transform.position -= delta * NearestSpeed * Time.deltaTime;
		
		previousPosition = transform.position;
	}
}
