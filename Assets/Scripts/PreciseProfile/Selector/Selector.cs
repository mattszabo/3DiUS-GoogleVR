using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PreciseProfile.Selector
{
    public class Selector : MonoBehaviour {

        private int _currentProfileIndex;
        private List<ProfileModel> _profileList = new List<ProfileModel>();
        private List<ProfileModel> _filteredProfileList = new List<ProfileModel>();

        private enum SelectorStates {
            None,
            SelectLeft,
            SelectRight,
            Loading,
            UpdateSelectorFilter
        }

        private enum Directions {
            Left = -1,
            Right = 1
        }

        private SelectorStates _selectorState;
        private string _selectorFilter;

        public void Start () {
            _selectorState = SelectorStates.None;
            _selectorFilter = "ALL";
        }

        private void UpdateProfilePicture() {
            var tex = _filteredProfileList[_currentProfileIndex].ProfilePictureTex;
            var profilePicture = transform.FindChild("ProfilePicture").gameObject;
            profilePicture.GetComponent<Renderer>().material.mainTexture = tex;
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
                case SelectorStates.Loading:
                    FilterSelectorProfiles("asdf");
                    UpdateProfilePicture();
                    break;
                case SelectorStates.UpdateSelectorFilter:
                    FilterSelectorProfiles("Marketing Consultant");
                    UpdateProfilePicture();
                    break;
                case SelectorStates.SelectLeft:
                    if(_filteredProfileList.Count > 0) {
                        UpdateCurrentProfileIndex((int)Directions.Left);
                        UpdateProfilePicture();
                    }
                    _selectorState = SelectorStates.None;
                    break;
                case SelectorStates.SelectRight:
                    if(_filteredProfileList.Count > 0) {
                        UpdateCurrentProfileIndex((int)Directions.Right);
                        UpdateProfilePicture();
                    }
                    _selectorState = SelectorStates.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateCurrentProfileIndex(int i)
        {
            _currentProfileIndex += i;
            if (_currentProfileIndex >= _filteredProfileList.Count) {
                _currentProfileIndex = _filteredProfileList.Count - 1;
            } else if (_currentProfileIndex <= 0) {
                _currentProfileIndex = 0;
            }
        }

        private void UpdateDisplay(string gameObjectName)
        {
            _selectorState = gameObjectName == "RightButton" ?
                SelectorStates.SelectRight : SelectorStates.SelectLeft;
        }

        private void OnEnable() {
            SelectorController.Direction += UpdateDisplay;
            SelectorController.AddProfile += AddProfile;
            SelectorController.DeleteProfile += DeleteProfile;
            ProfileService.PassProfileModels += LoadSelectorWithProfiles;
        }

        private void OnDisable() {
            SelectorController.Direction -= UpdateDisplay;
            SelectorController.AddProfile -= AddProfile;
            SelectorController.DeleteProfile -= DeleteProfile;
            ProfileService.PassProfileModels -= LoadSelectorWithProfiles;
        }

        private static void DeleteProfile(Transform transform) {
            ProfileService.DeleteProfile(transform);
        }

        private void AddProfile() {
            if(_filteredProfileList.Count > 0)
            {
                new ProfileService().AddProfile(_filteredProfileList[_currentProfileIndex]);
            }
        }

        private void LoadSelectorWithProfiles(List<ProfileModel> aProfileCollection) {
            _profileList = aProfileCollection;
            _selectorState = SelectorStates.Loading;
        }
        private void FilterSelectorProfiles(string aTitleFilter) {
            if (_selectorFilter == aTitleFilter) return;
            if(_selectorFilter.Equals("ALL")) {
                _filteredProfileList = _profileList;
            } else {
                _currentProfileIndex = 0;
                _filteredProfileList = _profileList.Where(p => p.Title.Equals(aTitleFilter)).ToList();
            }
            _selectorFilter = aTitleFilter;
        }

        public void UpdateSelectorFilter() {
            _selectorState = SelectorStates.UpdateSelectorFilter;
        }
    }
}
