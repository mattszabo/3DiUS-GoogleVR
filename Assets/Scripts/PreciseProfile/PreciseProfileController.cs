using UnityEngine;
using System.Collections;

public class PreciseProfileController : MonoBehaviour {

	public GameObject prefab;
	public delegate void Action(Transform transform);
	public static event Action Delete;
	public void AddProfile() {
		Instantiate(prefab);
	}

	public void DeleteProfile() {
		Delete(transform);
	}
}
