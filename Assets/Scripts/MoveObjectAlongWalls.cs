using UnityEngine;
using System.Collections;

namespace Moveable {
	public enum PointerType { GvrReticlePointer, GvrControllerPointer };	
}

public class MoveObjectAlongWalls : MonoBehaviour {

	public PointerType pointerType;

	public GameObject frontWall;
	public GameObject rightWall;
	public GameObject backWall;
	public GameObject leftWall;
	public GameObject ceiling;
	public GameObject floor;

	public GameObject currentWall;

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

		Ray pointerRay = new Ray (pointer.transform.position, pointer.transform.forward);

//		// we pick up objects at their center. so get the distance from the center
//		// to the edge and make sure we don't let the edge go through adjacent walls
//		Vector3 

		if (currentWall == backWall || currentWall == frontWall) {

			// place the object along the wall in a position relative to the pointer
			transform.position = pointerRay.GetPoint (Vector3.Distance (pointerRay.origin, currentWall.transform.position));
			// adjust the z position to match the z pos of the current wall (rotating around the pointers origin also moves the z position, which we want to keep the same as the wall)
			transform.position = new Vector3 (transform.position.x, transform.position.y, currentWall.transform.position.z - 0.3f);

			// set the forward and right position of the object.
			// when picking up with the pointer ray, we need to make sure these are set or the
			// object we're holding can be flipped/rotated
			transform.forward = currentWall.transform.forward;
			transform.right = -currentWall.transform.right;

			// move to left wall
			float halfWidth = currentWall.GetComponent<Renderer>().bounds.size.x / 2.0f;
			if (transform.position.x < currentWall.transform.position.x - halfWidth) {
				Debug.Log(currentWall.GetComponent<Renderer>().bounds.size.x);
				currentWall = leftWall;
			}

		} else if (currentWall == leftWall || currentWall == rightWall) {
			transform.forward = -currentWall.transform.forward;
			transform.right = -currentWall.transform.right;

			transform.position = pointerRay.GetPoint (Vector3.Distance (pointerRay.origin, currentWall.transform.position));
			transform.position = new Vector3 (currentWall.transform.position.x + 0.3f, transform.position.y, transform.position.z);
		}

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
