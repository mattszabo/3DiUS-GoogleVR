using UnityEngine;

[System.Serializable]
public class ProfileModel
{
	public string Name;
	public string Title;
	public string Bio;
	public string PhotoUrl;
	public string AuthenticatedUrl;
	public Texture2D ProfilePictureTex;

	public static ProfileModel CreateFromJson(string jsonString) {
		return JsonUtility.FromJson<ProfileModel> (jsonString);
	}
}
