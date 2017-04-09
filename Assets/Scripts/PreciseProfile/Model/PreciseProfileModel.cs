using UnityEngine;

[System.Serializable]
public class PreciseProfileModel {

	public string name;
	public string title;
	public string bio;
	public string photo_url;
	public string authenticated_url;
	public Texture2D profile_picture_tex;

	public static PreciseProfileModel CreateFromJson(string jsonString) {
		return JsonUtility.FromJson<PreciseProfileModel> (jsonString);
	}
}
