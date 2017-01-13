using UnityEngine;
using System.Collections;

namespace Moveable {
	public enum PointerType { GvrReticlePointer, GvrControllerPointer };	
}

public class MoveObjectAlongWalls : MonoBehaviour {


	public PointerType pointerType;
	private GameObject pointer;

	private bool isPickedUp;
	private bool isGravityEnabled;

	private Rigidbody rb;

	private Vector3 objectLastPosition;
	private Vector3 objectVelocity;

	void Start () {
		isPickedUp = false;
		pointer = GameObject.Find (pointerType.ToString());
		rb = GetComponent<Rigidbody> ();
		isGravityEnabled = (rb) ? rb.useGravity : false;
	}
		
	void Update () {
		if (isPickedUp) {
			FollowPointerAlongWall ();
			if (isGravityEnabled) {
				CalculateObjectVelocity ();
			}
		}
	}

	public void PickUpObject() {
		isPickedUp = true;
	}

	public void LetGoOfObject() {
		isPickedUp = false;
		if (isGravityEnabled) {
			ApplyMomentumToObject ();
		}
	}

	private void FollowPointerAlongWall() {
		Ray ray = new Ray (pointer.transform.position, pointer.transform.forward);
//		GameObject camera = GameObject.Find ("Main Camera");
		GameObject backWall = GameObject.Find ("BackWall");
		transform.position = ray.GetPoint (Vector3.Distance(ray.origin, backWall.transform.position));
		transform.position = new Vector3(transform.position.x, transform.position.y, backWall.transform.position.z - 0.3f);
		transform.forward = GameObject.Find("BackWall").transform.forward;
		transform.right = -GameObject.Find("BackWall").transform.right;


		// 1. determine which wall the camera is facing
		// 2. set object position at the back wall in relation to the camera (set x, y pos)
		// 3. change the transform.position of the object to that walls z location with an offset
		// 4. face the object to the opposite wall
		// 5. check colision with adjacent walls and stop movement
		// 6. when pointer changes walls, change orientation of object to point to opposite wall and set x, y pos with no collision
	}

	private void CalculateObjectVelocity() {
		objectVelocity = (transform.position - objectLastPosition) / Time.fixedDeltaTime;
		objectLastPosition = transform.position;
	}

	private void ApplyMomentumToObject() {
		float velocityMultiplier = 2.0f;
		rb.AddForce (
			objectVelocity.x * velocityMultiplier,
			objectVelocity.y * velocityMultiplier,
			objectVelocity.z * velocityMultiplier,
			ForceMode.Force
		);
	}
		
}
