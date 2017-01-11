using UnityEngine;
using System.Collections;

public class ImageViewer : MonoBehaviour {

	public string url = "http://api.precise.io/orgs/dius/public_profiles/mszabo";

	IEnumerator Start() {

		// get json from api
		WWW www = new WWW(url);
		yield return www;

		// parse json and create C# object
		PreciseProfileModel ppm = PreciseProfileModel.CreateFromJSON (www.text);

		// download the image from the photo_url
		www = new WWW (ppm.photo_url);
		yield return www;

		// apply the downloaded image as a texture
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = www.texture;
	}
}
