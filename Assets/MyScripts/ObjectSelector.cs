using GoogleARCore.HelloAR;
using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
    public class SelectModelButton : MonoBehaviour
    {

        public GameObject Model;
        public GameObject Controller;
        public Button[] Others;

        private HelloARController _scriptController;

        private void Start()
        {
            _scriptController = Controller.GetComponent<HelloARController>();
            _scriptController.modelPrefab = null;
            GetComponent<Button>().image.color = Color.white;
        }

        public void OnClick()
        {
            if (_scriptController.modelPrefab == Model)
            {
                _scriptController.modelPrefab = null;
                GetComponent<Button>().image.color = Color.white;
            }
            else
            {
                _scriptController.modelPrefab = Model;
                GetComponent<Button>().image.color = Color.gray;
                foreach (var button in Others)
                {
                    button.image.color = Color.white;
                }
            } 
                
        }
    }
}
