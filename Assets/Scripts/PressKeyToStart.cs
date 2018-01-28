using UnityEngine;
using UnityEngine.SceneManagement;

public class PressKeyToStart : MonoBehaviour
{
	[SerializeField] private string sceneToLoad = "0_0Tutorial";

	void Update ()
	{
		if(Input.anyKey)
		{
			SceneManager.LoadScene(sceneToLoad);
		}	
	}
}
