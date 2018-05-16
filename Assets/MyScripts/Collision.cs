using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
    public class Collision : MonoBehaviour {

        // Use this for initialization
        void Start () {
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            _ShowAndroidToastMessage("Collision with: " + other.gameObject.name);
        
        }
        private void OnTriggerEnter(Collider other)
        {
            _ShowAndroidToastMessage(gameObject.name + " toco a: " + other.gameObject.name);
        }

        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }


    }
}
