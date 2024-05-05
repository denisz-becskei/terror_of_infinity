using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    private float y = 0.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.localRotation = Quaternion.Euler(0, y, 0);
        y += 0.01f;
    }
}
