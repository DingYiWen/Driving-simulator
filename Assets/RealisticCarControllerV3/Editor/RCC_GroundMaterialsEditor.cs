//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RCC_GroundMaterials))]
public class RCC_GroundMaterialsEditor : Editor {

	RCC_GroundMaterials prop;

	Vector2 scrollPos;
	List<RCC_GroundMaterials.GroundMaterialFrictions> groundMaterials = new List<RCC_GroundMaterials.GroundMaterialFrictions>();

	Color orgColor;

	public override void OnInspectorGUI (){

		serializedObject.Update();
		prop = (RCC_GroundMaterials)target;
		orgColor = GUI.color;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Wheels Editor", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("This editor will keep update necessary .asset files in your project. Don't change directory of the ''Resources/RCCAssets''.", EditorStyles.helpBox);
		EditorGUILayout.Space();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false );

		EditorGUIUtility.labelWidth = 110f;
		//EditorGUIUtility.fieldWidth = 10f;

		GUILayout.Label("Ground Materials", EditorStyles.boldLabel);

		for (int i = 0; i < prop.frictions.Length; i++) {

			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.Space();

			if(prop.frictions[i].groundMaterial)
				EditorGUILayout.LabelField(prop.frictions[i].groundMaterial.name + (i == 0 ? " (Default)" : ""), EditorStyles.boldLabel);

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();

			prop.frictions[i].groundMaterial = (PhysicMaterial)EditorGUILayout.ObjectField("Physic Material", prop.frictions[i].groundMaterial, typeof(PhysicMaterial), false, GUILayout.Width(250f));
			prop.frictions[i].forwardStiffness = EditorGUILayout.FloatField("Forward Stiffness", prop.frictions[i].forwardStiffness, GUILayout.Width(250f));

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			prop.frictions[i].groundSound = (AudioClip)EditorGUILayout.ObjectField("Wheel Sound", prop.frictions[i].groundSound, typeof(AudioClip), false, GUILayout.Width(250f));
			prop.frictions[i].sidewaysStiffness = EditorGUILayout.FloatField("Sideways Stiffness", prop.frictions[i].sidewaysStiffness, GUILayout.Width(250f));

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			prop.frictions[i].groundParticles = (GameObject)EditorGUILayout.ObjectField("Wheel Particles", prop.frictions[i].groundParticles, typeof(GameObject), false, GUILayout.Width(250f));
			prop.frictions[i].slip = EditorGUILayout.FloatField("Slip", prop.frictions[i].slip, GUILayout.Width(250f));

			EditorGUILayout.Space();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			prop.frictions[i].damp = EditorGUILayout.FloatField("Damp", prop.frictions[i].damp, GUILayout.Width(250f));
			GUI.color = Color.red;		if(GUILayout.Button("Remove", GUILayout.Width(75f))){RemoveGroundMaterial(i);}	GUI.color = orgColor;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();

		}

		EditorGUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label("Terrain Ground Materials", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("useTerrainSplatMapForGroundFrictions"), new GUIContent("Use Terrain SplatMap For Ground Physics"), false);
		if(prop.useTerrainSplatMapForGroundFrictions){
			EditorGUILayout.PropertyField(serializedObject.FindProperty("terrainPhysicMaterial"), new GUIContent("Terrain Physic Material"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("terrainSplatMapIndex"), new GUIContent("Terrain Splat Map Index"), true);
		}
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();

		GUI.color = Color.cyan;

		if(GUILayout.Button("Create New Ground Material")){

			AddNewWheel();

		}

		if(GUILayout.Button("--< Return To Asset Settings")){

			OpenGeneralSettings();

		}

		GUI.color = orgColor;

		EditorGUILayout.EndScrollView();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Created by Buğra Özdoğanlar\nBoneCrackerGames", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		serializedObject.ApplyModifiedProperties();

		if(GUI.changed)
			EditorUtility.SetDirty(prop);

	}

	void AddNewWheel(){

		groundMaterials.Clear();
		groundMaterials.AddRange(prop.frictions);
		RCC_GroundMaterials.GroundMaterialFrictions newGroundMaterial = new RCC_GroundMaterials.GroundMaterialFrictions();
		groundMaterials.Add(newGroundMaterial);
		prop.frictions = groundMaterials.ToArray();

	}

	void RemoveGroundMaterial(int index){

		groundMaterials.Clear();
		groundMaterials.AddRange(prop.frictions);
		groundMaterials.RemoveAt(index);
		prop.frictions = groundMaterials.ToArray();

	}

	void OpenGeneralSettings(){

		Selection.activeObject =RCC_Settings.Instance;

	}

}
