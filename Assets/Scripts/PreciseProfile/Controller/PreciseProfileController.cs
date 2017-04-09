using UnityEngine;

public class PreciseProfileController : MonoBehaviour {

	public delegate void Action();
	public delegate void ActionTransform(Transform transform);
	public delegate void ActionGameObject(GameObject gameObject);

	public static event ActionTransform DeleteProfile;
	public static event ActionGameObject Direction;
	public static event Action AddProfile;

	public void DirectionButtonClick() {
	    if (Direction != null) Direction(gameObject);
	}

	public void AddProfileButtonClick() {
	    if (AddProfile != null) AddProfile();
	}
	
	public void DeleteProfileButtonClick() {
	    if (DeleteProfile != null) DeleteProfile(transform);
	}
}
