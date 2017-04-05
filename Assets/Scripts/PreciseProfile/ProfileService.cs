using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PreciseProfile
{
    public class ProfileService : MonoBehaviour {

        public delegate void PassProfileModelsDelegate(List<ProfileModel> profileCollection);
        public static event PassProfileModelsDelegate PassProfileModels;

        public GameObject ProfileView;

        private readonly List<string> _profileUrls = new List<string>();
        private static readonly List<string> _openProfileKeys = new List<string>();
        private readonly List<ProfileModel> _profileCollection = new List<ProfileModel>();

        public IEnumerator Start() {
            _profileUrls.Add("http://api.precise.io/orgs/dius/public_profiles/mszabo");
            _profileUrls.Add("http://api.precise.io/orgs/dius/public_profiles/dsummers");
            _profileUrls.Add("http://api.precise.io/orgs/dius/public_profiles/enash");
            // ProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/sbartlett");
            // ProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/kong");
            // ProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/nali");

            // Get each profile's text and profile texture... this currently takes a fair few seconds.

            foreach(var url in _profileUrls) {
                var www = new WWW(url);
                yield return www;
                var model =  ProfileModel.CreateFromJson(www.text);
                www = new WWW (model.PhotoUrl);
                yield return www;
                model.ProfilePictureTex = www.texture;
                _profileCollection.Add(model);
                Debug.Log("Added " + model.Name);
                LoadProfilesIntoSelector();
            }
        }

        private void LoadProfilesIntoSelector() {
            PassProfileModels(_profileCollection);
        }

        public List<ProfileModel> GetProfileCollections() {
            return _profileCollection;
        }

        internal static void DeleteProfile(Transform transform) {
            GameObject profile = transform.parent.gameObject;
            Destroy(profile);
            Debug.Log("Deleting " + profile.name);
            Debug.Log(_openProfileKeys);
            _openProfileKeys.Remove(profile.GetComponent<ProfileView>().GetPhotoUrl());
            Debug.Log(_openProfileKeys);
        }

        public void AddProfile(ProfileModel profileModel){
            var profileKey = profileModel.PhotoUrl;
            if (_openProfileKeys.Contains(profileKey)) return;
            var profile = Instantiate(ProfileView);
            profile.GetComponent<ProfileView>().Init(profileModel);
            //profile.GetComponent<Profile>().DisableBio();
            _openProfileKeys.Add(profileKey);
        }
    }
}

