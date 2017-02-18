// /*
// GET A COLLECTION OF PRECISE PROFILE MODELS
// */

// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class PreciseProfileCollection {

// 	private List<string> preciseProfileURLS = new List<string>();
	
// 	private List<PreciseProfileModel> preciseProfileCollection = new List<PreciseProfileModel>();

// 	public IEnumerator Init () {
		
// 		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/mszabo");
// 		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/dsummers");
// 		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/enash");
// 		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/sbartlett");
// 		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/kong");
// 		preciseProfileURLS.Add("http://api.precise.io/orgs/dius/public_profiles/nali");

// 		WWW www;
// 		foreach(string url in preciseProfileURLS) {
// 			Debug.Log(url);
// 			www = new WWW(url);
// 			yield return www;
// 			preciseProfileCollection.Add(PreciseProfileModel.CreateFromJSON(www.text));
// 		}
// 	}

// 	public List<PreciseProfileModel> GetPreciseProfileCollection() {
// 		return preciseProfileCollection;
// 	}
// }
