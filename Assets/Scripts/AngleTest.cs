using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour {

    public Transform target;

    private float angle;

	// Use this for initialization
	private void Start () {
        angle = 0f;
		
	}
	
	// Update is called once per frame
	private void Update () {
        angle = Vector3.Angle(transform.forward, target.position);
	}

    private void OnGUI() {
        GUI.Label(new Rect(10f, 10f, Screen.width / 5f, Screen.height / 5f), string.Format("Angle: {0}", angle));
    }
}
