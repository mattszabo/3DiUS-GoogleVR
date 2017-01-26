using UnityEngine;
using System.Collections;

public class MoveObjectAlongWalls : MonoBehaviour {

	public Material defaultMat;
	public Material selectedMat;
	public Material movingMat;

	private GameObject laserPointer;
	private GameObject alignedWall;

	private bool isPointedAt;
	private bool isPickedUp;
	private bool isPositionOnWallInitialised;

	private Vector3 objectLastPosition;
	private Vector3 objectVelocity;

	// layer mask to select any object with layer set to PickedUpObject (edit -> project settings -> tags & layers)
	int layerPickedUpObject = 1 << 8;
	int allLayersExceptPickedUpObject;

	GameObject[] walls;

	void Start () {
		isPickedUp = false;
		isPositionOnWallInitialised = false;
		laserPointer = GameObject.Find ("Laser");

		walls = GameObject.FindGameObjectsWithTag("Wall");

		// layer mask to select any object except those with layer set to PickedUpObject
		allLayersExceptPickedUpObject = ~layerPickedUpObject;
	}
		
	void Update () {
		if (GvrController.ClickButtonDown && isPointedAt) {
			isPickedUp = true;
		}
		if (GvrController.ClickButtonUp) {
			isPickedUp = false;
//			GetComponent<Renderer> ().material = defaultMat;
		}
		if (isPickedUp) {
//			GetComponent<Renderer> ().material = selectedMat;
			FollowPointerAlongWall ();
		}
	}

	public void PickUpObject() {
		isPickedUp = true;
	}

	private void FollowPointerAlongWall() {

		Vector3 v = GvrController.Orientation * Vector3.forward;

		Ray laserPointerRay = new Ray (laserPointer.transform.position, v);
//		Ray pointerRay = new Ray (v, pointer.transform.forward);
		RaycastHit hit = GetHitOfObjectBeingPointedAt (laserPointerRay);

		// ignore the picked up object being hit so we can check for wall collisions
		if(IsObjectAlignedToAWall()) {
			bool ignorePickedUpObject = true;
			hit = GetHitOfObjectBeingPointedAt(laserPointerRay, ignorePickedUpObject);
		}
			
		foreach (GameObject wall in walls) {
			if (IsAWallTagHit (hit, wall)) {
				AlignObjectToWall (wall, laserPointerRay, hit.point);
				UpdateObjectPositionToHitPoint (laserPointerRay, hit.point);
			}
		}
	}

	private RaycastHit GetHitOfObjectBeingPointedAt(Ray pointerRay, bool ignorePickedUpObject = false) {
		RaycastHit hit;
		if (ignorePickedUpObject) {
			Physics.Raycast (pointerRay, out hit, 140.0f, allLayersExceptPickedUpObject);
		} else {
			Physics.Raycast (pointerRay, out hit, 140.0f);
		}

		return hit;
	}

	private bool IsObjectAlignedToAWall() {
		return alignedWall ? true : false;
	}

	private void UpdateObjectPositionToHitPoint(Ray pointerRay, Vector3 point) {
		transform.position = pointerRay.GetPoint (Vector3.Distance (pointerRay.origin, point));
	}


	private void AlignObjectToWall(GameObject wall, Ray pointerRay, Vector3 point) {
		if (alignedWall != wall) {
			alignedWall = wall;

			transform.right = alignedWall.transform.right;

			if(transform.rotation != alignedWall.transform.rotation) {
				Quaternion w = alignedWall.transform.rotation;
				transform.rotation.Set (w.x, w.y, w.z, w.w);
			}
		}
	}

	private bool IsAWallTagHit(RaycastHit hit, GameObject wall) {
		return hit.collider.gameObject == wall;
	}
		
	public void SetPointedAt() {
		isPointedAt = true;
//		GetComponent<Renderer> ().material = selectedMat;
	}

	public void SetUnpointedAt() {
		isPointedAt = false;
//		GetComponent<Renderer> ().material = defaultMat;
	}
		
}
