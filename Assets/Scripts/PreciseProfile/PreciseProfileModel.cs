/*
THE STRUCTURE OF A PRECISE PROFILE
*/

using UnityEngine;

[System.Serializable]
public class PreciseProfileModel {

	public string name;
	public string title;
	public string bio;
	public string photo_url;
	public string authenticated_url;
	public Texture2D profilePictureTex;
	public int id;

	public static PreciseProfileModel CreateFromJSON(string jsonString) {
		return JsonUtility.FromJson<PreciseProfileModel> (jsonString);
	}
}
