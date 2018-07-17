//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RCC_ResetScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyUp(KeyCode.R)){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	
	}

}
