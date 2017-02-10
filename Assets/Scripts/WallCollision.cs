using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// attach me to a wall
public class WallCollision : MonoBehaviour {

	public delegate void Collided(GameObject wall);
	public static event Collided OnEnter;
	public static event Collided OnExit;

	void OnCollisionEnter(Collision col)
	{
		Debug.Log ("on collision enter");
		Debug.Log (col.gameObject.layer);
		// if (col.gameobject.layer == "laser")

		// this is 'on' the wall, we want to pass ourselves in the event, we tell the laser, you hit me!
		// OnCollidedEnter(gameObject);
	}

	void OnCollisionExit(Collision col)
	{
		Debug.Log ("on collision exit");
		Debug.Log (col.gameObject.layer);
		// this is 'on' the wall, we want to pass ourselves in the event, we tell the laser, you hit me!
		// OnCollidedExit(gameObject);
	}
}