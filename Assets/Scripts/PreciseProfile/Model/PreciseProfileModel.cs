using UnityEngine;

[System.Serializable]
public class PreciseProfileModel {

	public string Name;
	public string Title;
	public string Bio;
	public string PhotoUrl;
	public string AuthenticatedUrl;
	public Texture2D ProfilePictureTex;

	public static PreciseProfileModel CreateFromJson(string jsonString) {
		return JsonUtility.FromJson<PreciseProfileModel> (jsonString);
	}
}
