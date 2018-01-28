using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulser : MonoBehaviour
{
	[SerializeField] private float change = 0.2f;
	[SerializeField] private float cycleSpeed = 1.0f;

	private float t;
	
	// Update is called once per frame
	void Update ()
	{
		t += Time.deltaTime;
		float scale = 1.0f + (Mathf.Sin(t * cycleSpeed) * change);
		transform.localScale = new Vector3(scale, scale, scale);
	}
}
