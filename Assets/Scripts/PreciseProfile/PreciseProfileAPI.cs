using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
public class PreciseProfileAPI : MonoBehaviour {

	public string url = "http://api.precise.io/orgs/dius/public_profiles/mszabo";

	private enum PreciseProfileSections {
		ProfileName, ProfileTitle, ProfilePicture, ProfileBio
	};

	IEnumerator Start() {

		// get json from api
		WWW www = new WWW(url);
		yield return www;

		// parse json and create C# object
		PreciseProfileModel ppm = PreciseProfileModel.CreateFromJSON (www.text);

		// download the image from the photo_url
		www = new WWW (ppm.photo_url);
		yield return www;

		SetProfilePicture (www.texture);

		SetProfileObjectText (ppm.name, PreciseProfileSections.ProfileName.ToString());
		SetProfileObjectText (ppm.title, PreciseProfileSections.ProfileTitle.ToString());
		string bioString = ppm.bio.Substring(0, 265);
		bioString = Regex.Replace(bioString, ".{40}", "$0\n");
		SetProfileObjectText (bioString, PreciseProfileSections.ProfileBio.ToString());

	}

	// apply the downloaded image to the profile picture as a texture
	private void SetProfilePicture(Texture2D tex) {
		GameObject profilePicture = GameObject.Find(PreciseProfileSections.ProfilePicture.ToString());
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;

//		GameObject testCard = GameObject.Find("TestCard");
//		testCard.GetComponent<Renderer> ().material.mainTexture = tex;
	}

	private void SetProfileObjectText(string objectText, string objectLabel) {
		GameObject profileBio = GameObject.Find (objectLabel);
		profileBio.GetComponent<TextMesh> ().text = objectText;
	}
}
