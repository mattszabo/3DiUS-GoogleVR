using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PreciseProfileSelector : MonoBehaviour {

	private int _currentFilteredProfileIndex;
	private List<PreciseProfileModel> _profileCollection = new List<PreciseProfileModel>();
	private List<PreciseProfileModel> _filteredProfileCollection = new List<PreciseProfileModel>();
	
    private enum SelectorStates {
		None,
		SelectLeft,
		SelectRight
    }

    private enum Directions {
		Left = -1,
		Right = 1
    }

    private const string FILTER_ALL = "ALL";
    private const string FILTER_MARKETING_CONSULTANT = "Marketing Consultant";

	private SelectorStates _selectorState;

	public void Start () {

		_selectorState = SelectorStates.None;
	}

    private void UpdateProfilePicture() {
		var tex = _filteredProfileCollection[_currentFilteredProfileIndex].profile_picture_tex;
		var profilePicture = transform.Find("ProfilePicture").gameObject;
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;
	}

	public void SelectLeft() {
		_selectorState = SelectorStates.SelectLeft;
	}

	public void SelectRight() {
		_selectorState = SelectorStates.SelectRight;
	}

    private void Update () {
		switch(_selectorState) {
			case SelectorStates.None:
				break;
			case SelectorStates.SelectLeft:
				if(_profileCollection.Count > 0) {
					UpdateDisplayedProfile((int)Directions.Left);
				}
				_selectorState = SelectorStates.None;
				break;
			case SelectorStates.SelectRight:
				if(_profileCollection.Count > 0) {
					UpdateDisplayedProfile((int)Directions.Right);
				}
				_selectorState = SelectorStates.None;
				break;
		    default:
		        throw new ArgumentOutOfRangeException();
		}
	}

    private void UpdateDisplayedProfile(int i)
    {
        _currentFilteredProfileIndex += i;
		if (_currentFilteredProfileIndex >= _filteredProfileCollection.Count) {
			_currentFilteredProfileIndex = _filteredProfileCollection.Count - 1;
		} else if (_currentFilteredProfileIndex <= 0) {
			_currentFilteredProfileIndex = 0;
		}
        UpdateProfilePicture();
    }

    private void UpdateDisplay(GameObject gameObject) {
		if(gameObject.name == "RightButton") {
			_selectorState = SelectorStates.SelectRight;
		} else {
			_selectorState = SelectorStates.SelectLeft;
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
        if (_filteredProfileCollection.Count > 0)
        {
            PreciseProfileService.AddProfile(_filteredProfileCollection[_currentFilteredProfileIndex]);
        }
    }

	private void LoadSelectorWithProfiles(List<PreciseProfileModel> aProfileCollection) {
	    _profileCollection = aProfileCollection;
	    _filteredProfileCollection = FilterSelectorProfiles(FILTER_ALL);
	    UpdateProfilePicture ();
	}

	private List<PreciseProfileModel> FilterSelectorProfiles(string aTitleFilter) {
	    if(aTitleFilter == FILTER_ALL) {
            return _profileCollection;
        }
	    // reset index to avoid out of bounds index
        _currentFilteredProfileIndex = 0;
        return _profileCollection.Where(p => p.title == aTitleFilter).ToList();
    }
}
