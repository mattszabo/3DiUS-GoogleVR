/*
PRECISE PROFILE SELECTOR IS ESSENTIALLY A MANAGER FOR SEARCHING AND OPENING UP PROFILES
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PreciseProfileSelector : MonoBehaviour {

	private int currentProfileIndex;
	private List<PreciseProfileModel> profileCollection = new List<PreciseProfileModel>();
	private PreciseProfileService preciseProfileService;

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

	public void Start () {
		
		selectorState = SelectorStates.NONE;
		// preciseProfileService = new PreciseProfileService();
		// profileCollection = preciseProfileService.GetProfilesFromList();
	}

    private void UpdateProfilePicture() {
		Texture2D tex = profileCollection[currentProfileIndex].profilePictureTex;
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
				if(profileCollection.Count > 0) {
					UpdateCurrentProfileIndex((int)Directions.LEFT);
					UpdateProfilePicture();
				}
				selectorState = SelectorStates.NONE;
				break;
			case SelectorStates.SELECT_RIGHT:
				if(profileCollection.Count > 0) {
					UpdateCurrentProfileIndex((int)Directions.RIGHT);
					UpdateProfilePicture();
				}
				selectorState = SelectorStates.NONE;
				break;
			default:
				break;
		}
	}

    private void UpdateCurrentProfileIndex(int i)
    {
        currentProfileIndex += i;
		if (currentProfileIndex >= profileCollection.Count) {
			currentProfileIndex = profileCollection.Count - 1;
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
		PreciseProfileController.Direction += UpdateDisplay;
		PreciseProfileController.AddProfile += AddProfile;
		PreciseProfileController.DeleteProfile += DeleteProfile;
		PreciseProfileService.PassProfileModels += LoadSelectorWithProfiles;
	}

    void OnDisable() {
		PreciseProfileController.Direction -= UpdateDisplay;
		PreciseProfileController.AddProfile -= AddProfile;
		PreciseProfileController.DeleteProfile -= DeleteProfile;
		PreciseProfileService.PassProfileModels -= LoadSelectorWithProfiles;
	}

    private void DeleteProfile(Transform transform) {
        PreciseProfileService.DeleteProfile(transform);
    }

    private void AddProfile() {
		if(profileCollection.Count > 0) {
			PreciseProfileService.AddProfile(profileCollection[currentProfileIndex]);
		}
    }

	private void LoadSelectorWithProfiles(List<PreciseProfileModel> profileCollection) {
		this.profileCollection = profileCollection;
		UpdateProfilePicture ();
	}

	
}
