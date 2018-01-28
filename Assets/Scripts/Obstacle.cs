using UnityEngine;

public class Obstacle : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D coll)
	{
		Ship ship = coll.gameObject.GetComponent<Ship>();
		if(ship != null)
		{
			ship.Collision();
			
			LevelManager levelManager = FindObjectOfType<LevelManager>();
			if (levelManager != null)
			{
				levelManager.OnShipCollided();
			}
		}
	}
}
