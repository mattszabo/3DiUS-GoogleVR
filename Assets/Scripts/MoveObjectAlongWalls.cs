using UnityEngine;
using System.Collections;

namespace Moveable {
	public enum PointerType { GvrReticlePointer, GvrControllerPointer };	
}

public class MoveObjectAlongWalls : MonoBehaviour {

	public PointerType pointerType;

	private GameObject pointer;
	private GameObject currentWall;

	private bool isPickedUp;
	private bool isGravityEnabled;
	private bool isPositionOnWallInitialised;

	private Rigidbody rb;

	private Vector3 objectLastPosition;
	private Vector3 objectVelocity;

	GameObject[] walls;

	void Start () {
		isPickedUp = false;
		isPositionOnWallInitialised = false;
		pointer = GameObject.Find (pointerType.ToString());
		rb = GetComponent<Rigidbody> ();
		isGravityEnabled = (rb) ? rb.useGravity : false;

		walls = GameObject.FindGameObjectsWithTag("Wall");
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
		isPositionOnWallInitialised = false;
		if (isGravityEnabled) {
			ApplyMomentumToObject ();
		}
		currentWall = null;
	}

	private void FollowPointerAlongWall() {

		Ray pointerRay = new Ray (pointer.transform.position, pointer.transform.forward);
		RaycastHit hit;

		// Movement of object isn't fluid because Raycast doesn't go through the object to the wall.
		// So it only moves the object along the wall in spaces of width/2 or height/2
		if (Physics.Raycast (pointerRay, out hit, 40.0f)) {
			foreach (GameObject wall in walls) {
				if (hit.collider.gameObject == wall) {
					currentWall = wall;
					transform.position = pointerRay.GetPoint (Vector3.Distance (pointerRay.origin, hit.point));
					transform.forward = currentWall.transform.forward;
					transform.right = -currentWall.transform.right;
				}
			}
		}
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
