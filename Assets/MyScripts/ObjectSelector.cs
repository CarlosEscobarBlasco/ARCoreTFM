using GoogleARCore.HelloAR;
using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
    public class ObjectSelector : MonoBehaviour
    {

        public GameObject Model;
        public GameObject Controller;
        public Button[] Others;

        private HelloARController _scriptController;

        private void Start()
        {
            _scriptController = Controller.GetComponent<HelloARController>();
            _scriptController.virtualObject = null;
            GetComponent<Button>().image.color = Color.white;
        }

        public void OnClick()
        {
            if (_scriptController.virtualObject == Model)
            {
                _scriptController.virtualObject = null;
                GetComponent<Button>().image.color = Color.white;
            }
            else
            {
                _scriptController.virtualObject = Model;
                GetComponent<Button>().image.color = Color.gray;
                foreach (var button in Others)
                {
                    button.image.color = Color.white;
                }
            } 
                
        }
    }
}
