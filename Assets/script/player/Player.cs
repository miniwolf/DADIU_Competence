using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player: MonoBehaviour {

	public float runSpeed = 3.0f;

	public enum RotationAxes {
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

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