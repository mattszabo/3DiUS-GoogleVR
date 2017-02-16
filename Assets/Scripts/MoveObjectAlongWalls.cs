using System;
using UnityEngine;

public class MoveObjectAlongWalls : MonoBehaviour {

	public Material defaultMat;
	public Material selectedMat;
	public Material movingMat;

	private GameObject laserPointer;
	private GameObject currentWall;

	private bool isPointedAt;

	private Vector3 objectLastPosition;
	private Vector3 objectVelocity;

	enum ObjectStates
	{
		NONE,
		PICKED_UP,
        NOT_PICKED_UP,
        FOLLOWING_POINTER
    }

	ObjectStates objectState;

	readonly int layerWall = 1 << 10;

	private Renderer DeleteButtonRenderer;

	void Start () {
		laserPointer = GameObject.Find ("Laser");
		objectState = ObjectStates.NONE;
		DeleteButtonRenderer = transform.FindChild("DeleteButton").GetComponent<Renderer>();
		
	}
		
	void Update () {
		switch (objectState) {
			case ObjectStates.NONE:
				break;
			case ObjectStates.PICKED_UP:
			 	Profiler.BeginSample("--> PICKED_UP sample");
				SetFollowingPointer();
				Profiler.EndSample();
				break;
			case ObjectStates.FOLLOWING_POINTER:
				FollowPointerAlongWall();
				break;
			case ObjectStates.NOT_PICKED_UP:
				setNone();
				break;
			default:
				break;
		}
	}

    private void SetFollowingPointer()
    {
        objectState = ObjectStates.FOLLOWING_POINTER;
    }

    private void FollowPointerAlongWall() {
        Vector3 controllerOffset = new Vector3(0.35f, -1.0f, 0.0f);
        Ray laserPointerRay = new Ray (laserPointer.transform.position + controllerOffset, laserPointer.transform.forward);
		RaycastHit hitObject;

		// check if we're hitting an object and if we are, then check if it's a wall
		if (Physics.Raycast (laserPointerRay, out hitObject, 140.0f, layerWall)) {
			if (hitObject.collider.CompareTag("Wall")) {
				GameObject newWall = hitObject.collider.gameObject;
				if(currentWall != newWall) {
					AlignObjectToWall(newWall);
					currentWall = newWall;
				}
				UpdateObjectPositionToHitPoint (laserPointerRay, hitObject.point);
			}
		}
	}

	private void UpdateObjectPositionToHitPoint(Ray pointerRay, Vector3 point) {
        float distance = Vector3.Distance(pointerRay.origin, point);
        float objectDepth = transform.localScale.z;
        transform.position = pointerRay.GetPoint (distance - objectDepth);
	}

	public void AlignObjectToWall(GameObject wall) {
		transform.right = wall.transform.right;

		Quaternion alignedWallRotation = wall.transform.rotation;
		if (transform.rotation != alignedWallRotation) {
			Quaternion w = alignedWallRotation;
			transform.rotation = alignedWallRotation;
		}
		this.currentWall = wall;
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
