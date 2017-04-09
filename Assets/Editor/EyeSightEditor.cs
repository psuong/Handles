using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EyeSight))]
public class EyeSightEditor : Editor {

	private SerializedProperty viewDistanceProperty;
	private SerializedProperty viewAngleProperty;
	private EyeSight eyeSight;

	private void OnEnable() {
		viewDistanceProperty = serializedObject.FindProperty ("viewDistance");
		viewAngleProperty = serializedObject.FindProperty ("viewAngle");

		eyeSight = (EyeSight)target;
	}

	private void OnDisable() {
		
	}

	private void OnSceneGUI() {
		EditorGUI.BeginChangeCheck ();
		DrawSolidArc ();
		var distance = DrawViewDistance (eyeSight.transform.position, eyeSight.transform.forward);
		if (EditorGUI.EndChangeCheck()) {
			serializedObject.Update ();
			viewDistanceProperty.floatValue = distance;
			serializedObject.ApplyModifiedProperties ();
		}
	}

	private float DrawViewDistance(Vector3 origin, Vector3 direction) {
		var coneCapPosition = origin + direction * viewDistanceProperty.floatValue;
		Handles.color = Color.yellow;
		var position = Handles.FreeMoveHandle (coneCapPosition, Quaternion.identity, 0.15f, new Vector3 (0.5f, 0.5f, 0.5f), Handles.DotCap);

		return Vector3.Distance (position, origin);
	}

	private void DrawSolidArc() {
		Handles.color = new Color (0, 1, 1, 0.25f);
		Handles.DrawSolidArc (eyeSight.transform.position, 
			eyeSight.transform.up, 
			eyeSight.transform.forward, 
			viewAngleProperty.floatValue / 2, 
			viewDistanceProperty.floatValue
		);

		Handles.DrawSolidArc (eyeSight.transform.position, 
			eyeSight.transform.up, 
			eyeSight.transform.forward, 
			-viewAngleProperty.floatValue / 2, 
			viewDistanceProperty.floatValue
		);
	}
}
