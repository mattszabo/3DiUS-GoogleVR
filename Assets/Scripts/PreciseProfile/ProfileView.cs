/*
DISPLAY A PRECISE PROFILE
*/

using UnityEngine;
using System.Text.RegularExpressions;

public class ProfileView : MonoBehaviour
{

    private enum ProfileSections
    {
        ProfileName, ProfileTitle, ProfilePicture, ProfileBio
    };

    private ProfileModel profileModel;

    public void Init(ProfileModel profile)
    {
        profileModel = profile;
        SetProfilePicture(profile.ProfilePictureTex);
        SetProfileObjectText(profile.Name, ProfileSections.ProfileName.ToString());
        SetProfileObjectText(profile.Title, ProfileSections.ProfileTitle.ToString());
        var bioString = profile.Bio.Substring(0, 265);
        bioString = Regex.Replace(bioString, ".{40}", "$0\n");
        SetProfileObjectText(bioString, ProfileSections.ProfileBio.ToString());
    }

    private void SetProfilePicture(Texture2D tex)
    {
        var profilePicture = transform.FindChild("ProfilePicture").gameObject;
        profilePicture.GetComponent<Renderer>().material.mainTexture = tex;
    }

    private void SetProfileObjectText(string objectText, string objectLabel)
    {
        var profileSection = transform.FindChild(objectLabel + "Card").FindChild(objectLabel).gameObject;
        profileSection.GetComponent<TextMesh>().text = objectText;
    }

    public string GetPhotoUrl()
    {
        return profileModel.PhotoUrl;
    }

    //public void DisableBio()
    //{
    //    transform.FindChild("ProfileBioCard").gameObject.active = false;
    //}
}
