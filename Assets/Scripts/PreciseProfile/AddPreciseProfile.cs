using UnityEngine;
using System.Collections;

public class AddPreciseProfile : MonoBehaviour {

	public GameObject prefab;
	public void AddProfile() {
		Debug.Log("Add Profile");
		// Vector3 pos = new Vector3(0.0f, 0.0f, 9.0f);
		Instantiate(prefab);
	}
}
