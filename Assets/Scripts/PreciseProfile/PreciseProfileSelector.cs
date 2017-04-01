/*
PRECISE PROFILE SELECTOR HANDLES BROWSING THROUGH, FILTERING, AND OPENING UP PROFILES
*/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PreciseProfileSelector : MonoBehaviour {

	private int currentProfileIndex;
	private List<PreciseProfileModel> profileList = new List<PreciseProfileModel>();
	private List<PreciseProfileModel> filteredProfileList = new List<PreciseProfileModel>();

	enum SelectorStates {
		NONE,
		SELECT_LEFT,
		SELECT_RIGHT,
        LOADING,
		UPDATE_SELECTOR_FILTER
    }

	enum Directions {
		LEFT = -1,
		RIGHT = 1
    }

	private SelectorStates selectorState;
	private string selectorFilter;

	public void Start () {
		selectorState = SelectorStates.NONE;
		selectorFilter = "ALL";
	}

    private void UpdateProfile() {
        //var tex = filteredProfileList[currentProfileIndex].profilePictureTex;
        //var profilePicture = transform.FindChild("ProfilePicture").gameObject;
        //profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;

        var profile = transform.FindChild("PreciseProfile").GetComponent<PreciseProfile>();
        profile.Init(filteredProfileList[currentProfileIndex]);
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
			case SelectorStates.LOADING:
                FilterSelectorProfiles("Marketing Consultant");
                UpdateProfile();
				break;
			case SelectorStates.UPDATE_SELECTOR_FILTER:
				FilterSelectorProfiles("Marketing Consultant");
				UpdateProfile();
				break;
			case SelectorStates.SELECT_LEFT:
				if(filteredProfileList.Count > 0) {
					UpdateCurrentProfileIndex((int)Directions.LEFT);
					UpdateProfile();
				}
				selectorState = SelectorStates.NONE;
				break;
			case SelectorStates.SELECT_RIGHT:
				if(filteredProfileList.Count > 0) {
					UpdateCurrentProfileIndex((int)Directions.RIGHT);
					UpdateProfile();
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
		if (currentProfileIndex >= filteredProfileList.Count) {
			currentProfileIndex = filteredProfileList.Count - 1;
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
		if(filteredProfileList.Count > 0) {
			PreciseProfileService.AddProfile(filteredProfileList[currentProfileIndex]);
		}
    }

	private void LoadSelectorWithProfiles(List<PreciseProfileModel> aProfileCollection) {
		this.profileList = aProfileCollection;
		//selectorState = SelectorStates.LOADING;
	}
	private void FilterSelectorProfiles(string aTitleFilter) {
		if(selectorFilter != aTitleFilter) {
			if(selectorFilter == "ALL") {
				filteredProfileList = profileList;
			} else {
				currentProfileIndex = 0;
				this.filteredProfileList = profileList.Where(p => p.title == aTitleFilter).ToList();
			}
		    selectorFilter = aTitleFilter;
		}
        Debug.Log("SF: " + selectorFilter);
    }

	public void UpdateSelectorFilter() {
		selectorState = SelectorStates.UPDATE_SELECTOR_FILTER;
	}
}
