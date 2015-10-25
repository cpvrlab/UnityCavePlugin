using UnityEngine;
using System.Collections;

namespace Cave
{
    public class Eyes : MonoBehaviour
    {

        private CaveMain _main;

        // Use this for initialization
        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}