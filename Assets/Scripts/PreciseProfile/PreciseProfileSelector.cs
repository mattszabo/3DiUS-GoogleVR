/*
PRECISE PROFILE SELECTOR HANDLES BROWSING THROUGH, FILTERING, AND OPENING UP PROFILES
*/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PreciseProfileSelector : MonoBehaviour {

	private int currentProfileIndex;
	private List<PreciseProfileModel> profileCollection = new List<PreciseProfileModel>();
	private List<PreciseProfileModel> displayProfileCollection = new List<PreciseProfileModel>();
	
	private PreciseProfileService preciseProfileService;

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
		// Debug.Log("starting service");
		// preciseProfileService = new PreciseProfileService();
		// Debug.Log("getting models");
		// profileCollection = preciseProfileService.GetProfileModels();
		// Debug.Log("got models");
		// Debug.Log(profileCollection);
		// UpdateProfilePicture();

		// CoroutineWithData cd = new CoroutineWithData(this, preciseProfileService.Init() );
		// yield return cd.coroutine;
		// profileCollection = (List<PreciseProfileModel>)cd.result;
		// UpdateProfilePicture();
	}

    private void UpdateProfilePicture() {
		Texture2D tex = displayProfileCollection[currentProfileIndex].profilePictureTex;
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
			case SelectorStates.LOADING:
				FilterSelectorProfiles("Marketing Consultant");
				UpdateProfilePicture ();
				break;
			case SelectorStates.UPDATE_SELECTOR_FILTER:
				FilterSelectorProfiles("Marketing Consultant");
				UpdateProfilePicture ();
				break;
			case SelectorStates.SELECT_LEFT:
				if(displayProfileCollection.Count > 0) {
					UpdateCurrentProfileIndex((int)Directions.LEFT);
					UpdateProfilePicture();
				}
				selectorState = SelectorStates.NONE;
				break;
			case SelectorStates.SELECT_RIGHT:
				if(displayProfileCollection.Count > 0) {
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
		if (currentProfileIndex >= displayProfileCollection.Count) {
			currentProfileIndex = displayProfileCollection.Count - 1;
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
		if(displayProfileCollection.Count > 0) {
			PreciseProfileService.AddProfile(displayProfileCollection[currentProfileIndex]);
		}
    }

	private void LoadSelectorWithProfiles(List<PreciseProfileModel> aProfileCollection) {
		this.profileCollection = aProfileCollection;
		selectorState = SelectorStates.LOADING;
	}
	private void FilterSelectorProfiles(string aTitleFilter) {
		if(selectorFilter != aTitleFilter) {
			if(selectorFilter == "ALL") {
				displayProfileCollection = profileCollection;
			} else {
				currentProfileIndex = 0;
				this.displayProfileCollection = profileCollection.Where(p => p.title == aTitleFilter).ToList();
			}
		}
		selectorFilter = aTitleFilter;
	}

	public void UpdateSelectorFilter() {
		selectorState = SelectorStates.UPDATE_SELECTOR_FILTER;
	}
}
