using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using GoogleARCore;
using JetBrains.Annotations;
using UnityEngine.XR;
using GoogleARCore;


public class Connecting : MonoBehaviour {
    public Camera MainCamera;
//    private GameObject[] ElementList = new GameObject[0];
    public Text IpText;

    // Conection
    private TcpListener _listner;
    private const int Port = 8010;
    private bool _stop;
    private List<TcpClient> clients = new List<TcpClient>();
    //This must be the-same with SEND_COUNT on the client
    private const int SEND_RECEIVE_COUNT = 20;

    // Info
    private float[] _cameraTransformInfo;
    private HashSet<GameObject> _elements = new HashSet<GameObject>();
    private int _elementIndex = 0;

    
    private void Start()
    {
        Application.runInBackground = true;
        //Start WebCam coroutine
        StartCoroutine(InitAndWaitForWebCamTexture());       
    }


    //Converts the data size to byte array and put result to the fullBytes array
    void byteLengthToFrameByteArray(int byteLength, byte[] fullBytes)
    {
        //Clear old data
        Array.Clear(fullBytes, 0, fullBytes.Length);
        //Convert int to bytes
        byte[] bytesToSendCount = BitConverter.GetBytes(byteLength);
        //Copy result to fullBytes
        bytesToSendCount.CopyTo(fullBytes, 0);
    }

    IEnumerator InitAndWaitForWebCamTexture()
    {
        
        // Connect to the server
        _listner = new TcpListener(IPAddress.Any, Port);
        _listner.Start();
        IpText.text = Network.player.ipAddress;
        //Start sending coroutine
        StartCoroutine(SenderCor());
        yield return null;
    }

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    IEnumerator SenderCor()
    {

        bool isConnected = false;
        TcpClient client = null;
        NetworkStream stream = null;

        // Wait for client to connect in another Thread 
        Loom.RunAsync(() =>
        {
            while (!_stop)
            {
                // Wait for client connection
                client = _listner.AcceptTcpClient();
                // We are connected
                clients.Add(client);

                isConnected = true;
                // Get a stream object for reading and writing
                stream = client.GetStream();
            }
        });

        //Wait until client has connected
        while (!isConnected)
        {
            yield return null;
        }

        bool readyToGetFrame = true;

        byte[] cameraTextureLength = new byte[SEND_RECEIVE_COUNT];

        while (!_stop)
        {
            //Wait for End of frame
            yield return endOfFrame;
          
            // Capture the camera texture
            RenderTexture targetTexture = MainCamera.targetTexture;
            RenderTexture.active = targetTexture;
            Texture2D texture2D = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
            texture2D.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            texture2D.Apply();
            byte[] cameraTextureBytes = texture2D.EncodeToPNG();
            DestroyImmediate(texture2D);
            byteLengthToFrameByteArray(cameraTextureBytes.Length, cameraTextureLength);
            
            //Wait until we are ready to get new frame(Until we are done sending data)
            while (!readyToGetFrame)
            {
                yield return null;
            }
            //Set readyToGetFrame false
            readyToGetFrame = false;

            Loom.RunAsync(() =>
            {
                              
                // Send the mobile camera texture
                stream.Write(cameraTextureLength, 0, cameraTextureLength.Length);
                stream.Write(cameraTextureBytes, 0, cameraTextureBytes.Length);
                
                // Set readyToGetFrame true
                readyToGetFrame = true;
            });

        }
    }

    private String TransformVector2String(float[] vector)
    {
        String result="";
        foreach (var parameter in vector)
        {
            result += parameter+" ";
        }

        return result;
    }
    
    // stop everything
    private void OnApplicationQuit()
    {
        _stop = true;
        
        if (_listner != null)
        {
            _listner.Stop();
        }

        foreach (TcpClient c in clients)
            c.Close();
    }
    
}
