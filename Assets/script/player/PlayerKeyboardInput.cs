using UnityEngine;
using System.Collections;

public class PlayerKeyboardInput : MonoBehaviour {

	public float runSpeed = 3.0f;

	void Update() {
		// part 1 - move character
		float change = runSpeed * Time.deltaTime;
		if( Input.GetKey(KeyCode.D) ) {
			transform.Translate(new Vector3(change, 0, 0));
		} else if( Input.GetKey(KeyCode.A) ) {
			transform.Translate(new Vector3(-change, 0, 0));
		} else if( Input.GetKey(KeyCode.S) ) {
			transform.Translate(new Vector3(0, 0, -change));
		} else if( Input.GetKey(KeyCode.W) ) {
			transform.Translate(new Vector3(0, 0, change));
		}
	}
}