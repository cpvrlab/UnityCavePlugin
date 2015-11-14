using UnityEngine;
using System.Collections;

public class CameraContainer : MonoBehaviour {

    private int _lastMaincameraID = 0;

	// Use this for initialization
	void Start () {
        
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
	}
}
