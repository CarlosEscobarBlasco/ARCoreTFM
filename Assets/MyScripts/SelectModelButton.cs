using GoogleARCore.HelloAR;
using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
    public class SelectModelButton : MonoBehaviour
    {

        public GameObject Model;
        public GameObject Controller;
        public GameObject Panel;

        private HelloARController _scriptController;

        private void Start()
        {
            _scriptController = Controller.GetComponent<HelloARController>();
        }

        public void OnClick()
        {
            if (_scriptController.modelPrefab == Model)
            {
                _scriptController.modelPrefab = null;
                Panel.SetActive(false);
            }
            else
            {
                _scriptController.modelPrefab = Model;
                Panel.SetActive(true);
                Panel.transform.position = new Vector3(transform.position.x,transform.position.y,1);
                
            } 
                
        }
    }
}
