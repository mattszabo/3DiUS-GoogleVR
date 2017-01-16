using UnityEngine;
using System.Collections;

namespace Moveable {
	public enum PointerType { GvrReticlePointer, GvrControllerPointer };	
}

public class MoveObjectAlongWalls : MonoBehaviour {

	public PointerType pointerType;

	private GameObject pointer;
	private GameObject alignedWall;

	private bool isPickedUp;
	private bool isGravityEnabled;
	private bool isPositionOnWallInitialised;

	private Rigidbody rb;

	private Vector3 objectLastPosition;
	private Vector3 objectVelocity;

	int objectLayerMask = 1 << 8;

	GameObject[] walls;

	void Start () {
		isPickedUp = false;
		isPositionOnWallInitialised = false;
		pointer = GameObject.Find (pointerType.ToString());
		rb = GetComponent<Rigidbody> ();
		isGravityEnabled = (rb) ? rb.useGravity : false;

		walls = GameObject.FindGameObjectsWithTag("Wall");

		objectLayerMask = ~objectLayerMask;
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
	}

	private void FollowPointerAlongWall() {

		Ray pointerRay = new Ray (pointer.transform.position, pointer.transform.forward);
		RaycastHit hit = GetHitOfObjectBeingPointerAt (pointerRay);

		// ignore the picked up object being hit so we can check for wall collisions
		if(IsObjectAlignedToAWall()) {
			bool ignorePickedUpObject = true;
			hit = GetHitOfObjectBeingPointerAt(pointerRay, ignorePickedUpObject);
		}
			
		foreach (GameObject wall in walls) {
			if (IsAWallTagHit (hit, wall)) {
				AlignObjectToWall (wall, pointerRay, hit.point);
				UpdateObjectPosition (pointerRay, hit.point);
			}
		}
	}

	private RaycastHit GetHitOfObjectBeingPointerAt(Ray pointerRay, bool ignorePickedUpObject = false) {
		RaycastHit hit;
		if (ignorePickedUpObject) {
			Physics.Raycast (pointerRay, out hit, 40.0f, objectLayerMask);
		} else {
			Physics.Raycast (pointerRay, out hit, 40.0f);
		}

		return hit;
	}

	private bool IsObjectAlignedToAWall() {
		return alignedWall ? true : false;
	}

	private void UpdateObjectPosition(Ray pointerRay, Vector3 point) {
		transform.position = pointerRay.GetPoint (Vector3.Distance (pointerRay.origin, point));
	}


	private void AlignObjectToWall(GameObject wall, Ray pointerRay, Vector3 point) {
		alignedWall = wall;

		transform.forward = alignedWall.transform.forward;
		transform.right = -alignedWall.transform.right;
	}

	private bool IsAWallTagHit(RaycastHit hit, GameObject wall) {
		return hit.collider.gameObject == wall;
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
