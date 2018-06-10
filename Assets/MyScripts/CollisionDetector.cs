using MyScripts.Net;
using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
    public class CollisionDetector : MonoBehaviour
    {

        public Sender Sender;
        
        private void OnTriggerEnter(Collider other)
        {
            Sender.SetCollisionMessage("Collision");
        }
        
        private void OnTriggerExit(Collider other)
        {
            Sender.SetCollisionMessage("");
        }

        
    }
}
