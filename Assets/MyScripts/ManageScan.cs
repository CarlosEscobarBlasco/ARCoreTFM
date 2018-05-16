using GoogleARCore;
using GoogleARCore.HelloAR;
using UnityEngine;
using UnityEngine.UI;

namespace MyScripts
{
	public class ManageScan : MonoBehaviour {
		public GameObject Controller;
		public GameObject ArCoreDevice;
		public GameObject ModelSelector;
		public GameObject SnackBar;
		public GameObject PointCloud;

		private ARCoreSession _sesion;
		private bool _enable = true;

		void Start () {
			_sesion = ArCoreDevice.GetComponent<ARCoreSession>();
		}
	

		public void ChangeScanStatus()
		{
			_enable = !_enable;
			_sesion.SessionConfig.EnablePlaneFinding = _enable;
			ModelSelector.SetActive(_enable);
			SnackBar.SetActive(_enable);
			PointCloud.SetActive(_enable);
			_sesion.OnEnable();
			GetComponentInChildren<Text>().text = _enable ? "Done" : "Edit";
			if (!_enable) Controller.GetComponent<HelloARController>().modelPrefab = null; 
		}

	}
}
