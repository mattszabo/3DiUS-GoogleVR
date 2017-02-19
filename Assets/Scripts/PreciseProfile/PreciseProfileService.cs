using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PreciseProfileService : MonoBehaviour {

	public delegate void PassProfileModelsDelegate(List<PreciseProfileModel> profileCollection);
	public static event PassProfileModelsDelegate PassProfileModels;

	private List<string> profileURLS = new List<string>();
	private static List<string> openProfiles = new List<string>();
	private List<PreciseProfileModel> profileCollection = new List<PreciseProfileModel>();

	public PreciseProfileService() {
		
	}
	public IEnumerator Start() {
		
		profileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/mszabo");
		profileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/dsummers");
		profileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/enash");
		// preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/sbartlett");
		// preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/kong");
		// preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/nali");

		// Get each profile's text and profile texture... this currently takes a fair few seconds.

		WWW www;
		foreach(string url in profileURLS) {
			www = new WWW(url);
			yield return www;
			PreciseProfileModel model =  PreciseProfileModel.CreateFromJSON(www.text);
			www = new WWW (model.photo_url);
			yield return www;
			model.profilePictureTex = www.texture;
			profileCollection.Add(model);
			Debug.Log("Added " + model.name);
		}

		LoadProfilesIntoSelector();
		// Debug.Log("WWW");
		// WWW www = (WWW)WwwYield("https://precise-photos-prod-us.s3.amazonaws.com/56ed9ffff79b4739299b786dcb880400").Current;
		// Debug.Log(www);

		// foreach(string url in profileURLS) {
		// 	CoroutineWithData cd = new CoroutineWithData(this, WwwYield(url) );
		// 	yield return cd.coroutine;
		// 	Debug.Log("result is " + cd.result);  //  'success' or 'fail'
		// 	profileCollection.Add((PreciseProfileModel)cd.result);
		// }

		// yield return profileCollection;
	}

	// public IEnumerator GetProfileModels() {
	// 	Init();
	// 	yield return profileCollection;
	// }

	// public IEnumerator WwwYield(String url) {
	// 	WWW www = new WWW(url);
	// 	yield return www;
	// 	if (String.IsNullOrEmpty(www.error)) {
	// 		PreciseProfileModel model = PreciseProfileModel.CreateFromJSON(www.text);
	// 		www = new WWW (model.photo_url);
	// 		if (String.IsNullOrEmpty(www.error)) {
	// 			model.profilePictureTex = www.texture;
	// 			yield return model;
	// 		} else {
	// 			yield return www.error;
	// 		}
	// 	} else {
	// 		yield return www.error;
	// 	}
	// }

    private void LoadProfilesIntoSelector() {
		PassProfileModels(profileCollection);
    }

    public List<PreciseProfileModel> GetProfileCollections() {
		return profileCollection;
	}

	internal static void DeleteProfile(Transform transform) {
		GameObject profile = transform.parent.gameObject;
		Destroy(profile);
		Debug.Log("Deleting " + profile.name);
		Debug.Log(openProfiles);
		openProfiles.Remove(profile.GetComponent<PreciseProfile>().GetPhotoUrl());
		Debug.Log(openProfiles);
	}

    internal static void AddProfile(PreciseProfileModel preciseProfileModel){
		if(!openProfiles.Contains(preciseProfileModel.photo_url)) {
			GameObject profile = Instantiate(Resources.Load("PreciseProfile")) as GameObject;
			profile.GetComponent<PreciseProfile>().Init(preciseProfileModel);
			openProfiles.Add(preciseProfileModel.photo_url);
		}
    }
}

// public class CoroutineWithData {
//      public Coroutine coroutine { get; private set; }
//      public object result;
//      private IEnumerator target;
//      public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
//          this.target = target;
//          this.coroutine = owner.StartCoroutine(Run());
//      }
 
//      private IEnumerator Run() {
//          while(target.MoveNext()) {
//              result = target.Current;
//              yield return result;
//          }
//      }
//  }
