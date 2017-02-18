using UnityEngine;
using System.Collections;

public class PreciseProfileSelectorController : MonoBehaviour {

	public delegate void Action(GameObject GameObject);
	public static event Action Direction;

	public void ButtonClick() {
		Direction(gameObject);
	}
}
