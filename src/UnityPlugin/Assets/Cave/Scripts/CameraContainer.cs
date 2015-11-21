using UnityEngine;
using System.Collections;
using Cave;

public class CameraContainer : MonoBehaviour {

    private int _lastMaincameraID = 0;
    private CaveMain _main;
    private Vector3 posWithSensitity;

    // Use this for initialization
    void Start () {
        _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        posWithSensitity = new Vector3(_main.EyesSettings.MovementSensivity.x, _main.EyesSettings.MovementSensivity.y, _main.EyesSettings.MovementSensivity.z);
    }
	
	// Update is called once per frame
	void Update () {
	    
        if(Camera.main.gameObject.GetInstanceID() != _lastMaincameraID)
        {
            _lastMaincameraID = Camera.main.gameObject.GetInstanceID();
            transform.parent = Camera.main.transform;
            transform.localPosition = Vector3.zero;

            Debug.Log("set new camera position");
        }

    
        this.transform.localPosition = Vector3.Scale(API.Instance.Eyes.transform.position,posWithSensitity);
        
	}
}
