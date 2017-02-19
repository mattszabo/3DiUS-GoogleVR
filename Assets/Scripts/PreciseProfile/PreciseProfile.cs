/*
DISPLAY A PRECISE PROFILE
*/

using UnityEngine;
using System.Text.RegularExpressions;

public class PreciseProfile : MonoBehaviour {

	private enum PreciseProfileSections {
		ProfileName, ProfileTitle, ProfilePicture, ProfileBio
	};

	private PreciseProfileModel profileModel;

	public void Init(PreciseProfileModel profile) {
		profileModel = profile;
		SetProfilePicture (profile.profilePictureTex);
		SetProfileObjectText (profile.name, PreciseProfileSections.ProfileName.ToString());
		SetProfileObjectText (profile.title, PreciseProfileSections.ProfileTitle.ToString());
		string bioString = profile.bio.Substring(0, 265);
		bioString = Regex.Replace(bioString, ".{40}", "$0\n");
		SetProfileObjectText (bioString, PreciseProfileSections.ProfileBio.ToString());
	}

	private void SetProfilePicture(Texture2D tex) {
		GameObject profilePicture = transform.FindChild("ProfilePicture").gameObject;
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;
	}

	private void SetProfileObjectText(string objectText, string objectLabel) {
		GameObject profileBio = transform.FindChild(objectLabel+"Card").FindChild(objectLabel).gameObject;
		profileBio.GetComponent<TextMesh> ().text = objectText;
	}	

	public string GetPhotoUrl() {
		return profileModel.photo_url;
	}
}
