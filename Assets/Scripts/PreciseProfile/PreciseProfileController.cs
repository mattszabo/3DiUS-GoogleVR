/*
CONTROLLER FOR ALL PRECISE PROFILE RELATED LOGIC
*/

using UnityEngine;

public class PreciseProfileController : MonoBehaviour {

	public delegate void Action();
	public delegate void ActionTransform(Transform transform);
	public delegate void ActionGameObject(GameObject gameObject);

	public static event ActionTransform DeleteProfile;
	public static event ActionGameObject Direction;
	public static event Action AddProfile;

	public void DirectionButtonClick() {
		Direction(gameObject);
	}

	public void AddProfileButtonClick() {
		AddProfile();
	}
	
	public void DeleteProfileButtonClick() {
		DeleteProfile(transform);
	}

}
