using UnityEngine;

public class Spinner : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 5.0f;

	public float RotationSpeed {  get { return rotationSpeed; } set { rotationSpeed = value; } }

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
