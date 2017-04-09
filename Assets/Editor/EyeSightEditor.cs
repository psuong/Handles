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

        var positiveAngle = DrawViewAngle(eyeSight.transform.position, eyeSight.transform.position + Vector3.forward * eyeSight.viewDistance, eyeSight.transform.forward, eyeSight.viewAngle);
        var negativeAngle = DrawViewAngle(eyeSight.transform.position, eyeSight.transform.position + Vector3.forward * eyeSight.viewDistance, eyeSight.transform.forward, -eyeSight.viewAngle);

		if (EditorGUI.EndChangeCheck()) {
			serializedObject.Update ();
			viewDistanceProperty.floatValue = distance;
            viewAngleProperty.floatValue = positiveAngle + negativeAngle;
			serializedObject.ApplyModifiedProperties ();
		}
	}

	private float DrawViewDistance(Vector3 origin, Vector3 direction) {
		var handlePosition = origin + direction * viewDistanceProperty.floatValue;
		Handles.color = Color.yellow;
		var position = Handles.FreeMoveHandle (handlePosition, Quaternion.identity, 0.15f, new Vector3 (0.5f, 0.5f, 0.5f), Handles.DotCap);

		return Vector3.Distance (position, origin);
	}

    private float DrawViewAngle(Vector3 origin, Vector3 destination, Vector3 forward, float angle) {
        var direction = destination - origin;
        var rotation = Quaternion.Euler(new Vector3(0f, angle / 2, 0f));

        var newDirection = rotation * direction;
        var position = newDirection + origin;
        var nextPosition = Handles.FreeMoveHandle(position, Quaternion.identity, 0.15f, Vector3.one / 2, Handles.DotCap);

        var resultantDirection = nextPosition - origin;

        Handles.DrawLine(origin, origin + resultantDirection);
        // Handles.DrawLine(origin, origin + direction);

        var alpha = Vector3.Dot(direction, resultantDirection) / (direction.magnitude * resultantDirection.magnitude);
        var returnedAngle = Mathf.Acos(alpha) * Mathf.Rad2Deg;

        Handles.Label(position, string.Format("Angle: {0}", returnedAngle * 2));
        return returnedAngle;
    }

	private void DrawSolidArc() {
		Handles.color = new Color (0, 1, 1, 0.1f);
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
