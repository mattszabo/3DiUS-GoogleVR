using UnityEngine;
using UnityEngine.EventSystems;

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
	readonly int layerPickedUpObject = 1 << 8;
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
		if (isPickedUp) {
			FollowPointerAlongWall ();
		}
	}

	public void PickUpObject() {
		isPickedUp = true;
	}

	private void FollowPointerAlongWall() {

        Vector3 controllerOffset = new Vector3(0.0f, 1.65f, 0.0f);
        Ray laserPointerRay = new Ray (laserPointer.transform.position - controllerOffset, laserPointer.transform.forward);
		RaycastHit hit = GetHitOfObjectBeingPointedAt (laserPointerRay);

		// ignore the picked up object being hit so we can check for wall collisions
		if(IsObjectAlignedToAWall()) {
			bool ignorePickedUpObject = true;
			hit = GetHitOfObjectBeingPointedAt(laserPointerRay, ignorePickedUpObject);
		}
			
		foreach (GameObject wall in walls) {
			if (IsAWallTagHit (hit, wall)) {
				AlignObjectToWall (wall, hit.point);
				UpdateObjectPositionToHitPoint (laserPointerRay, hit.point);
			}
		}
	}

    public void FollowPointer() {
        isPickedUp = true;
    }

    public void UnfollowPointer(){
        isPickedUp = false;
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
		return this.alignedWall ? true : false;
	}

	private void UpdateObjectPositionToHitPoint(Ray pointerRay, Vector3 point) {
        float distance = Vector3.Distance(pointerRay.origin, point);
        float objectDepth = transform.localScale.z;
        transform.position = pointerRay.GetPoint (distance - objectDepth);
	}


	private void AlignObjectToWall(GameObject wall, Vector3 point) {
		if (this.alignedWall != wall) {
            this.alignedWall = wall;

			transform.right = this.alignedWall.transform.right;

            Quaternion alignedWallRotation = this.alignedWall.transform.rotation;

            if (transform.rotation != alignedWallRotation) {
				Quaternion w = alignedWallRotation;
                transform.rotation.Set(w.x, w.y, w.z, w.w);
                //transform.rotation = alignedWallRotation;

            }
		}
	}

	private bool IsAWallTagHit(RaycastHit hit, GameObject wall) {
		return hit.collider.gameObject == wall;
	}
		
	public void SetPointedAt() {
		isPointedAt = true;
	}

	public void SetUnpointedAt() {
		isPointedAt = false;
	}
		
}
