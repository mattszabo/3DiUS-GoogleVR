﻿using System.Text.RegularExpressions;
using UnityEngine;

public class PreciseProfile : MonoBehaviour {

	private enum PreciseProfileSections {
		ProfileName, ProfileTitle, ProfileBio
	}

	private PreciseProfileModel _profileModel;

	public void Init(PreciseProfileModel profile) {
		_profileModel = profile;
		SetProfilePicture (profile.profile_picture_tex);
		SetProfileObjectText (profile.name, PreciseProfileSections.ProfileName.ToString());
		SetProfileObjectText (profile.title, PreciseProfileSections.ProfileTitle.ToString());
		var bioString = profile.bio.Substring(0, 265);
		bioString = Regex.Replace(bioString, ".{40}", "$0\n");
		SetProfileObjectText (bioString, PreciseProfileSections.ProfileBio.ToString());
	}

	private void SetProfilePicture(Texture tex) {
		var profilePicture = transform.Find("ProfilePicture").gameObject;
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;
	}

	private void SetProfileObjectText(string objectText, string objectLabel) {
		var profileBio = transform.Find(objectLabel+"Card").Find(objectLabel).gameObject;
		profileBio.GetComponent<TextMesh> ().text = objectText;
	}	

	public string GetPhotoUrl() {
		return _profileModel.photo_url;
	}
}
