//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;

public class RCC_InitLoad : MonoBehaviour {

	[InitializeOnLoad]
	public class InitOnLoad {

		static InitOnLoad(){

			if(!EditorPrefs.HasKey("RCC3.0fInstalled")){
				EditorPrefs.SetInt("RCC3.0fInstalled", 1);
				EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing and using Realistic Car Controller. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started");
			}

		}

	}

}
