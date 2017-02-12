using UnityEngine;

public class MoveObjectAlongWalls : MonoBehaviour {

	public Material defaultMat;
	public Material selectedMat;
	public Material movingMat;

	private GameObject laserPointer;
	private GameObject currentWall;

	private bool isPointedAt;
	private bool isPickedUp;
	private bool isPointerPointingAtWall;

	private Vector3 objectLastPosition;
	private Vector3 objectVelocity;

	enum ObjectStates
	{
		NONE,
		PICKED_UP,
        NOT_PICKED_UP
    }

	ObjectStates objectState;

	// layer mask to select any object with layer set to PickedUpObject (edit -> project settings -> tags & layers)
	readonly int layerPickedUpObject = 1 << 8;
	readonly int layerWall = 1 << 10;
	int allLayersExceptPickedUpObject;

	void OnEnable() {
		WallCollision.OnEnter += PointingAtWall;
		WallCollision.OnExit += NotPointingAtWall;
	}

	void OnDisable() {
		CleanupDelegates();
	}

	void Start () {
		isPickedUp = false;
		laserPointer = GameObject.Find ("Laser");
		objectState = ObjectStates.NONE;
		
		// layer mask to select any object except those with layer set to PickedUpObject
		allLayersExceptPickedUpObject = ~layerPickedUpObject;
	}
		
	void Update () {
		switch (objectState) {
			case ObjectStates.NONE:
				break;
			case ObjectStates.PICKED_UP:
			 	Profiler.BeginSample("--> PICKED_UP sample");
				FollowPointerAlongWall();
				Profiler.EndSample();
				break;
			case ObjectStates.NOT_PICKED_UP:
				setNone();
				break;
			default:
				break;
		}
	}

	private void FollowPointerAlongWall() {
        Vector3 controllerOffset = new Vector3(0.35f, -1.0f, 0.0f);
        Ray laserPointerRay = new Ray (laserPointer.transform.position + controllerOffset, laserPointer.transform.forward);
		RaycastHit objectBehindPickedUpObject;

		if(Physics.Raycast (laserPointerRay, out objectBehindPickedUpObject, 140.0f, layerWall) &&
				objectBehindPickedUpObject.collider.CompareTag("Wall")) {
			UpdateObjectPositionToHitPoint (laserPointerRay, objectBehindPickedUpObject.point);
			//Check for new wall to re-align here. Only if I can't update to point through this object when hoevering over new wall.
		}
	}

	private void UpdateObjectPositionToHitPoint(Ray pointerRay, Vector3 point) {
        float distance = Vector3.Distance(pointerRay.origin, point);
        float objectDepth = transform.localScale.z;
        transform.position = pointerRay.GetPoint (distance - objectDepth);
	}


	// If picking up na object, this only gets called on a new wall if the
	// pointer is not aiming at this object being picked up
	private void PointingAtWall(GameObject wall) {
		if(objectState == ObjectStates.PICKED_UP) {
			isPointerPointingAtWall = true;
			AlignObjectToWall(wall);
		}
	}

	public void NotPointingAtWall(GameObject wall) {
		isPointerPointingAtWall = false;
	}

	public void AlignObjectToWall(GameObject wall) {
		if(this.currentWall != wall) {
			transform.right = wall.transform.right;

			Quaternion alignedWallRotation = wall.transform.rotation;
			if (transform.rotation != alignedWallRotation) {
				Quaternion w = alignedWallRotation;
				transform.rotation = alignedWallRotation;
			}
			this.currentWall = wall;
		}
	}

	private void CleanupDelegates() {
		WallCollision.OnEnter -= PointingAtWall;
		WallCollision.OnExit -= NotPointingAtWall;
	}

	void OnDestroy() {
		Debug.Log("Deleting profile");
		CleanupDelegates();
	}
		
	public void setNone() {
		objectState = ObjectStates.NONE;
	}
	public void SetPointedAt() {
		isPointedAt = true;
	}

	public void SetNotPointedAt() {
		isPointedAt = false;
	}

	public void setPickedUp() {
		objectState = ObjectStates.PICKED_UP;
	}	

	public void setNotPickedUp() {
		objectState = ObjectStates.NOT_PICKED_UP;
	}
}
