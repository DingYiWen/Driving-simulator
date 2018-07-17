//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Mirror")]
public class RCC_Mirror : MonoBehaviour {

	private Camera cam;
	private RCC_CarControllerV3 carController;
	
	void InvertCamera () {

		cam = GetComponent<Camera>();
		cam.ResetWorldToCameraMatrix ();
		cam.ResetProjectionMatrix ();
		cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
		carController = GetComponentInParent<RCC_CarControllerV3>();

	}
	
	void OnPreRender () {
		GL.invertCulling = true;
	}
	
	void OnPostRender () {
		GL.invertCulling = false;
	}

	void Update(){

		if(!cam){
			InvertCamera();
			return;
		}

		cam.enabled = carController.canControl;

	}

}
