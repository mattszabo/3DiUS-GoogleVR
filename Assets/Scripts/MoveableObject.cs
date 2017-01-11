using UnityEngine;
using System.Collections;

public enum PointerType { GvrReticlePointer, GvrControllerPointer };

public class MoveableObject : MonoBehaviour {


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
			FollowPointer ();
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

	private void FollowPointer() {
		Ray ray = new Ray (pointer.transform.position, pointer.transform.forward);
		transform.position = ray.GetPoint (7.0f);
		transform.LookAt (GameObject.Find("Main Camera").transform);
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
