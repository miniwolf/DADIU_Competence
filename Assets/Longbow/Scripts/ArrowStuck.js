#pragma strict
//THIS SCRIPT IS USED FOR ARROWS TO STICK INTO OBJECTS
private var hit : RaycastHit;
var hitSound : AudioClip; //sound to play when arrow hits a surface

function Update() {
	//Only check for obstacles to stick to if the arrow is moving
	if(GetComponent.<Rigidbody>().velocity.magnitude > 0.5) {
		CheckForObstacles();
	}
	else {
		enabled = false;
	}
}
function CheckForObstacles() {
	//the length of the raycast needs to be greater the faster the arrow is traveling, in order for it to register
	var myVelocity : float = GetComponent.<Rigidbody>().velocity.magnitude;
	//get length of raycast based on velocity of arrow | based on this, arrows with too little of a velocity will not stick
	var raycastLength : float = myVelocity * 0.03f;
	
	if(Physics.Raycast(transform.position, transform.forward, hit, raycastLength)) {
		GetComponent.<AudioSource>().Stop();
		GetComponent.<AudioSource>().clip = hitSound;
		GetComponent.<AudioSource>().Play();
	
		//since we need to disable the boxCollider and freeze the rigidbody for the arrow to stick,
		//we must add our own force to any moveable objects (objects with rigidbodies) that the arrow hits
		if(hit.transform.GetComponent.<Rigidbody>()) {
			//add force to object hit
			hit.transform.GetComponent.<Rigidbody>().AddForce(transform.forward * myVelocity * 10);
		}
		
		//disable arrow's collider | if you leave the arrow's collider on, while inside another object, it may have strange effects
		GetComponent(BoxCollider).enabled = false;
		//if you require the boxCollider to remain active, say for picking up the arrow, you can instead set the collider to trigger:
		//GetComponent(BoxCollider).isTrigger = true;
		
		//freeze rigidbody
		GetComponent.<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		//position arrow
		transform.position = hit.point;
		//parent arrow to object | if the object the arrow hits moves, this will make the arrow move with it
		transform.parent = hit.transform;
		
		//once the arrow is stuck, disable this script
		enabled = false;
	}
	else {
		//make arrows top-heavy | on arrows with little velocity (when the bow is not drawn back very far) the arrows will rotated toward the ground
		var newRot : Quaternion = transform.rotation;
		newRot.SetLookRotation(GetComponent.<Rigidbody>().velocity);
		transform.rotation = newRot;
	}
}
function OnCollisionEnter(collision : Collision) {
	if(GetComponent.<Rigidbody>().velocity.magnitude > 5) {
		if(collision.transform.tag != "Player") {
			GetComponent(BoxCollider).enabled = false;
			//freeze rigidbody
			GetComponent.<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			//position arrow
			for (var contact : ContactPoint in collision.contacts) {
		         transform.position = contact.point;
			}
			//parent arrow to object | if the object the arrow hits moves, this will make the arrow move with it
			transform.parent = collision.transform;
			
			enabled = false;
		}
	}
}
