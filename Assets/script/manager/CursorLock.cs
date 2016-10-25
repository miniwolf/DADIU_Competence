using UnityEngine;

namespace Assets.script.manager {
	public class CursorLock: MonoBehaviour {
		private GUITexture gt;
		private bool wasLocked;

		protected void Start() {
			gt = GetComponent<GUITexture>();
		}

		private void DidLockCursor() {
			Debug.Log("Locking cursor");
			gt.enabled = false;
		}

		private void DidUnlockCursor() {
			Debug.Log("Unlocking cursor");
			gt.enabled = true;
		}
		protected void OnMouseDown() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		protected void Update() {
			if ( Input.GetKeyDown("escape") ) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}

			if ( Cursor.lockState != CursorLockMode.Locked && wasLocked) {
				wasLocked = false;
				DidUnlockCursor();
			} else if ( Cursor.lockState != CursorLockMode.Locked && !wasLocked) {
				wasLocked = true;
				DidLockCursor();
			}
		}
	}
}