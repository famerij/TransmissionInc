using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameDebug : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene("SplashScreen");
		}
		
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			int fromKey = (int)KeyCode.Alpha1;
			string[] levels = new string[] {
				"0_0Tutorial",
				"0_1Prologue",
				"1_Level1",
				"2_Level2",
				"3_Level3",
				"4_Epilogue"
			};

			for (int i = fromKey; i < fromKey + levels.Length; i++)
			{
				if (Input.GetKeyDown((KeyCode)i))
				{
					SceneManager.LoadScene(levels[i - fromKey]);
					return;
				}
			}
		}
	}
}
