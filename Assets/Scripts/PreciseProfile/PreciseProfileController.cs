/*
CONTROLLER FOR ALL PRECISE PROFILE RELATED LOGIC
*/

using UnityEngine;

public class PreciseProfileController : MonoBehaviour {

	// public GameObject prefab;
	public delegate void Action();
	public delegate void ActionTransform(Transform transform);
	public delegate void ActionGameObject(GameObject gameObject);

	public static event ActionTransform Delete;
	public static event ActionGameObject Direction;
	public static event Action AddProfile;
	
	// public void AddProfile() {
	// 	Instantiate(prefab);
	// }

	public void DeleteProfile() {
		Delete(transform);
	}

	public void DirectionButtonClick() {
		Direction(gameObject);
	}

	public void AddProfileButtonClick() {
		AddProfile();
	}

}
