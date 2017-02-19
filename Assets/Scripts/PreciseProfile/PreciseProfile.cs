/*
DISPLAY A PRECISE PROFILE
*/

using UnityEngine;
using System.Text.RegularExpressions;

public class PreciseProfile : MonoBehaviour {

	// private PreciseProfileModel profile;

	private enum PreciseProfileSections {
		ProfileName, ProfileTitle, ProfilePicture, ProfileBio
	};

	void OnEnable() {
		PreciseProfileController.Delete += DeletePreciseProfile;
	}

    void OnDisable() {
		PreciseProfileController.Delete -= DeletePreciseProfile;
	}

	public void Init(PreciseProfileModel profile) {
		SetProfilePicture (profile.profilePictureTex);
		SetProfileObjectText (profile.name, PreciseProfileSections.ProfileName.ToString());
		SetProfileObjectText (profile.title, PreciseProfileSections.ProfileTitle.ToString());
		string bioString = profile.bio.Substring(0, 265);
		bioString = Regex.Replace(bioString, ".{40}", "$0\n");
		SetProfileObjectText (bioString, PreciseProfileSections.ProfileBio.ToString());
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
