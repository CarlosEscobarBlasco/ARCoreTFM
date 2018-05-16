using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
	public class CameraInfo : MonoBehaviour {
		public GameObject Camera;
		

		// Update is called once per frame
		void Update () {
			this.gameObject.GetComponent<Text>().text = "|x: "+Camera.transform.position.x + " || y:" +  Camera.transform.position.y+" |";
		}
	}
}
