using UnityEngine;

public class PreciseProfileSelectorController : MonoBehaviour {

	public delegate void Action(GameObject GameObject);
	public static event Action Direction;
	public GameObject prefab;

	public void ButtonClick() {
		Direction(gameObject);
	}

	public void AddProfile() {
		Instantiate(prefab);
	}
}
