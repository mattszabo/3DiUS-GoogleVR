using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PreciseProfileSelector : MonoBehaviour {

	private List<string> preciseProfileURLS = new List<string>();
	
	private List<PreciseProfileModel> preciseProfileCollection = new List<PreciseProfileModel>();

	public IEnumerator Start () {
		Debug.Log("start");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/mszabo");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/dsummers");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/enash");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/sbartlett");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/kong");
		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/nali");

		// Get each profile's text and profile texture... this currently takes a fair few seconds.
		WWW www;
		foreach(string url in preciseProfileURLS) {
			www = new WWW(url);
			yield return www;
			PreciseProfileModel model =  PreciseProfileModel.CreateFromJSON(www.text);
			www = new WWW (model.photo_url);
			yield return www;
			model.profilePictureTex = www.texture;

			preciseProfileCollection.Add(model);
		}

		SetProfilePicture (preciseProfileCollection[0].profilePictureTex);
	}
	
    private void SetProfilePicture(Texture2D tex) {
		Debug.Log("Yew");
		GameObject profilePicture = transform.FindChild("ProfilePicture").gameObject;
		profilePicture.GetComponent<Renderer> ().material.mainTexture = tex;
	}

    // Update is called once per frame
    void Update () {
		
	}
}
