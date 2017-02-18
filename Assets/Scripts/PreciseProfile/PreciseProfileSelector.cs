using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreciseProfileSelector : MonoBehaviour {

	private List<string> preciseProfileURLS = new List<string>();
	
	private List<PreciseProfileModel> preciseProfileCollection = new List<PreciseProfileModel>();

	private int currentProfileIndex;

	enum SelectorStates {
		NONE,
		SELECT_LEFT,
		SELECT_RIGHT
    }

	enum Directions {
		LEFT = -1,
		RIGHT = 1
    }

	private SelectorStates selectorState;

	public IEnumerator Start () {
		currentProfileIndex = 0;
		selectorState = SelectorStates.NONE;

		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/mszabo");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/dsummers");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/enash");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/sbartlett");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/kong");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/nali");

		// Get each profile's text and profile texture... this currently takes a fair few seconds.
		WWW www;
		foreach(string url in preciseProfileURLS) {
			www = new WWW(url);
			yield return www;
			PreciseProfileModel model =  PreciseProfileModel.CreateFromJSON(www.text);
			www = new WWW (model.photo_url);
			yield return www;
			model.profilePictureTex = www.texture;

			preciseProfileCollection.Add(model);
			Debug.Log("Added " + model.name);
		}

		UpdateProfilePicture ();
	}

    private void UpdateProfilePicture() {
		Texture2D tex = preciseProfileCollection[currentProfileIndex].profilePictureTex;
		GameObject profilePicture = transform.FindChild("ProfilePicture").gameObject;
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;
	}

	public void SelectLeft() {
		selectorState = SelectorStates.SELECT_LEFT;
	}

	public void SelectRight() {
		selectorState = SelectorStates.SELECT_RIGHT;
	}
	
    void Update () {
		switch(selectorState) {
			case SelectorStates.NONE:
				break;
			case SelectorStates.SELECT_LEFT:
				UpdateCurrentProfileIndex((int)Directions.LEFT);
				UpdateProfilePicture();
				selectorState = SelectorStates.NONE;
				break;
			case SelectorStates.SELECT_RIGHT:
				UpdateCurrentProfileIndex((int)Directions.RIGHT);
				UpdateProfilePicture();
				selectorState = SelectorStates.NONE;
				break;
			default:
				break;
		}
	}

    private void UpdateCurrentProfileIndex(int i)
    {
        currentProfileIndex += i;
		if (currentProfileIndex >= preciseProfileCollection.Count) {
			currentProfileIndex = preciseProfileCollection.Count - 1;
		} else if (currentProfileIndex <= 0) {
			currentProfileIndex = 0;
		}
    }

    private void UpdateDisplay(GameObject gameObject) {
		if(gameObject.name == "RightButton") {
			selectorState = SelectorStates.SELECT_RIGHT;
		} else {
			selectorState = SelectorStates.SELECT_LEFT;
		}
    }

	void OnEnable() {
		PreciseProfileSelectorController.Direction += UpdateDisplay;
	}

	void OnDisable() {
		PreciseProfileSelectorController.Direction -= UpdateDisplay;
	}
}
