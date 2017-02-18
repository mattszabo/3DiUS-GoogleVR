﻿/*
DISPLAY A PRECISE PROFILE
*/

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
public class PreciseProfile : MonoBehaviour {

	public string url = "http://api.precise.io/orgs/dius/public_profiles/mszabo";

	private enum PreciseProfileSections {
		ProfileName, ProfileTitle, ProfilePicture, ProfileBio
	};

	void OnEnable() {
		PreciseProfileController.Delete += DeletePreciseProfile;
	}

	void OnDisable() {
		PreciseProfileController.Delete -= DeletePreciseProfile;
	}

	IEnumerator Start() {

		// get json from api
		WWW www = new WWW(url);
		yield return www;

		// parse json and create C# object
		// PreciseProfileModel ppm = PreciseProfileModel.CreateFromJSON (www.text);

		// download the image from the photo_url
		// www = new WWW (ppm.photo_url);
		yield return www;

		SetProfilePicture (www.texture);

		// SetProfileObjectText (ppm.name, PreciseProfileSections.ProfileName.ToString());
		// SetProfileObjectText (ppm.title, PreciseProfileSections.ProfileTitle.ToString());
		// string bioString = ppm.bio.Substring(0, 265);
		// bioString = Regex.Replace(bioString, ".{40}", "$0\n");
		// SetProfileObjectText (bioString, PreciseProfileSections.ProfileBio.ToString());

	}

	// apply the downloaded image to the profile picture as a texture
	private void SetProfilePicture(Texture2D tex) {
		GameObject profilePicture = transform.FindChild("ProfilePicture").gameObject;
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;
	}

	private void SetProfileObjectText(string objectText, string objectLabel) {
		GameObject profileBio = transform.FindChild(objectLabel+"Card").FindChild(objectLabel).gameObject;
		profileBio.GetComponent<TextMesh> ().text = objectText;
	}

	public void DeletePreciseProfile(Transform transform) {
		GameObject preciseProfile = transform.parent.gameObject;
		Destroy(preciseProfile);
	}
}
