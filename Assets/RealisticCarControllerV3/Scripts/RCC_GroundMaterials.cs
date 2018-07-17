//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class RCC_GroundMaterials : ScriptableObject {
	
	#region singleton
	public static RCC_GroundMaterials instance;
	public static RCC_GroundMaterials Instance{	get{if(instance == null) instance = Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterials; return instance;}}
	#endregion

	[System.Serializable]
	public class GroundMaterialFrictions{
		
		public PhysicMaterial groundMaterial;
		public float forwardStiffness = 1f;
		public float sidewaysStiffness = 1f;
		public float slip = .25f;
		public float damp = 1f;
		public GameObject groundParticles;
		public AudioClip groundSound;

	}
		
	public GroundMaterialFrictions[] frictions;

	public bool useTerrainSplatMapForGroundFrictions = false;
	public PhysicMaterial terrainPhysicMaterial;
	public int[] terrainSplatMapIndex;

}


