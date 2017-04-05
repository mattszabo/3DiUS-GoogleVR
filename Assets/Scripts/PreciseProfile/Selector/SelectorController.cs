using UnityEngine;

namespace PreciseProfile.Selector
{
    public class SelectorController : MonoBehaviour {

        public delegate void Action();
        public delegate void ActionTransform(Transform transform);
        public delegate void ActionDirection(string gameObjectName);

        public static event ActionTransform DeleteProfile;
        public static event ActionDirection Direction;
        public static event Action AddProfile;

        public void DirectionButtonClick() {
            Direction(gameObject.name);
        }

        public void AddProfileButtonClick() {
            AddProfile();
        }
	
        public void DeleteProfileButtonClick() {
            DeleteProfile(transform);
        }

    }
}
