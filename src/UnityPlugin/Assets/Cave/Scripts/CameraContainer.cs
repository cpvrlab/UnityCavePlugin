using UnityEngine;
using System.Collections;
using Cave;

public class CameraContainer : MonoBehaviour {

    private int _lastMaincameraID = 0;
    private Vector3 posWithSensitity;

    // Use this for initialization
    void Start () {
        posWithSensitity = new Vector3(API.Instance.Cave.EyesSettings.MovementSensivity.x, API.Instance.Cave.EyesSettings.MovementSensivity.y, API.Instance.Cave.EyesSettings.MovementSensivity.z);
    }
	
	// Update is called once per frame
	void Update () {
	    
        if(Camera.main.gameObject.GetInstanceID() != _lastMaincameraID)
        {
            _lastMaincameraID = Camera.main.gameObject.GetInstanceID();
            transform.parent = Camera.main.transform;
            transform.localPosition = Vector3.zero;
        }
    
        transform.localPosition = Vector3.Scale(API.Instance.Eyes.transform.localPosition, posWithSensitity);
	}
}
