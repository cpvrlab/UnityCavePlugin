using UnityEngine;
using System.Collections;

namespace Cave
{
    public class Eyes : MonoBehaviour
    {

        private Main _main;

        // Use this for initialization
        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<Main>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}