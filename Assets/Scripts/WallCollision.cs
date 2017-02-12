using UnityEngine;

/* attach me to a wall */
public class WallCollision : MonoBehaviour {

	public delegate void Collided(GameObject wall);
	public delegate void NotCollided();
	public static event Collided OnEnter;
	public static event Collided OnExit;

	public void OnCollisionEnter(object gameObj) {
		OnEnter(gameObject);
	}

	public void OnCollisionExit(object gameObj) {
		OnExit(gameObject);
	}
}