using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyScripts.Net
{
    public class Sender : MonoBehaviour {
        public Camera MainCamera;
        public Text IpText;

        // Conection
        private TcpListener _listner;
        private const int Port = 8010;
        private bool _stop;
        private List<TcpClient> clients = new List<TcpClient>();
    
        // Stores the camera texture
        private Texture2D cameraTexture;
    
        //This must be the-same with SEND_COUNT on the client
        private const int SEND_RECEIVE_COUNT = 20;
        
        string collisionMessage = "";

        private void Start()
        {
//        Application.runInBackground = true;
            StartCoroutine(Connect());       
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

        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

        IEnumerator Connect()
        {

            bool isConnected = false;
            TcpClient client;
            NetworkStream stream = null;

            // Connect to the server
            _listner = new TcpListener(IPAddress.Any, Port);
            _listner.Start();
            IpText.text = "Connect to: " + Network.player.ipAddress;
        
            // Wait for client to connect in another Thread 
            Loom.RunAsync(() =>
            {
                while (!_stop)
                {
                    // Wait for client connection
                    client = _listner.AcceptTcpClient();
                
                    // connected
                    clients.Add(client);
                    isConnected = true;
                    IpText.text = "Connected";
                
                    // Get a stream object for reading and writing
                    stream = client.GetStream();
                }
            });

            //Wait until client has connected
            while (!isConnected)
            {
                yield return null;
            }

            StartCoroutine(SendBytes(stream));

        }

        IEnumerator SendBytes(Stream stream)
        {
            bool readyToGetFrame = true;
            byte[] cameraTextureLength = new byte[SEND_RECEIVE_COUNT];
            byte[] collisionMessageLength = new byte[SEND_RECEIVE_COUNT];

            while (!_stop)
            {
                //Wait for End of frame
                yield return endOfFrame;

                // Send collision information
                
                byte[] collisionMessageBytes = Encoding.ASCII.GetBytes(collisionMessage);
                byteLengthToFrameByteArray(collisionMessageBytes.Length, collisionMessageLength);
                
                // Capture the camera texture
                byte[] cameraTextureBytes = GetCameraBytes();
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
                    
                    // Send camera information
                    stream.Write(collisionMessageLength, 0, collisionMessageLength.Length);
                    stream.Write(collisionMessageBytes, 0, collisionMessageBytes.Length);
                    
                    // Send the mobile camera texture
                    stream.Write(cameraTextureLength, 0, cameraTextureLength.Length);
                    stream.Write(cameraTextureBytes, 0, cameraTextureBytes.Length);

                    // Set readyToGetFrame true
                    readyToGetFrame = true;
                });
            }
        }

        private byte[] GetCameraBytes()
        {
            RenderTexture targetTexture = MainCamera.targetTexture;
            RenderTexture.active = targetTexture;
            cameraTexture = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
            cameraTexture.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            cameraTexture.Apply();
            byte[] cameraTextureBytes = cameraTexture.EncodeToPNG();
            return cameraTextureBytes;
        }

        public void SetCollisionMessage(string message)
        {
            collisionMessage = message;
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
}
