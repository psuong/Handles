using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSight : MonoBehaviour {

	[Range(0f, 360f)]
	public float viewAngle = 160f;
	public float viewDistance = 20f;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        var position = transform.position + Vector3.forward;

        var direction = position - transform.position;
        var rotation = Quaternion.Euler(new Vector3(0f, viewAngle / 2, 0f));

        direction = rotation * direction;
        direction = direction.normalized * viewDistance;

        position = direction + transform.position;
        Gizmos.DrawSphere(position, 0.3f);
    }
}
