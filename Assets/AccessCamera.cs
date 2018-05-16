using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessCamera : MonoBehaviour
{

	public Text textureInfo;
	
	private WebCamTexture _webCam;
	private Texture2D currentTexture;
	private Color pixelColor;
	
	
	// Use this for initialization
	void Start () {
		_webCam = new WebCamTexture {deviceName = WebCamTexture.devices[0].name};
		print(_webCam.isPlaying);
		//currentTexture = new Texture2D(_webCam.width, _webCam.height);
	}
	
	// Update is called once per frame
	void Update ()
	{
		print(_webCam.isPlaying);
//		currentTexture.SetPixels(_webCam.GetPixels());
//		pixelColor = currentTexture.GetPixel(10, 10);
//		textureInfo.text = "R: "+pixelColor.r+" G: "+pixelColor.g+" B: "+pixelColor.b;
	}
}
