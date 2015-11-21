using UnityEngine;
using System.Collections;
using Cave;

public class BaseBehaviour : MonoBehaviour {

    protected CaveMain _main;
    protected CameraManager _cameraManager;
    protected FrustumManager _frustumManager;

    public void Awake()
    {
        _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        _cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
        _frustumManager = GameObject.FindWithTag("FrustumManager").GetComponent<FrustumManager>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
