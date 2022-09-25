using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace net.EthanTFH.BTSGameJam
{
    public class CameraModeTwo : MonoBehaviour
    {
        // Camera Variables
        private Transform targetPlayer;
        private float distanceFromTarget = 4.0f;

        private CameraModeOne normalController;
        private Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            if (normalController == null)
                normalController = gameObject.GetComponent<CameraModeOne>();

            if (cam == null)
                cam = Camera.main;

            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.509804f, 0.509804f, 0.509804f);
            Debug.Log("CamerModeTwo has been enabled!.");
        }

        // Update is called once per frame
        void Update()
        {
            if (targetPlayer == null || targetPlayer.transform == null)
            {
                if (PlayerController.LocalPlayerInstance)
                    targetPlayer = PlayerController.LocalPlayerInstance.transform;
                return;
            }

            distanceFromTarget -= Input.mouseScrollDelta.y;
            distanceFromTarget = Mathf.Clamp(distanceFromTarget, 3.0f, 10.0f);

            transform.position = new Vector3(distanceFromTarget, targetPlayer.position.y, targetPlayer.position.z);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            GameObject[] objects = GameObject.FindGameObjectsWithTag("StandardObject");
            GameObject[] o2 = GameObject.FindGameObjectsWithTag("Moveable");
            GameObject[] o3 = GameObject.FindGameObjectsWithTag("Player");

            if (o2.Length > 0)
                objects = objects.Concat(o2).ToArray();

            if (o3.Length > 0)
                objects = objects.Concat(o3).ToArray();

            foreach(GameObject o in objects)
            {
                if(o.GetComponent<Renderer>().material.HasFloat("_ClippingPlaneX"))
                {
                    o.GetComponent<Renderer>().material.SetFloat("_ClippingPlaneZ", -100);
                    o.GetComponent<Renderer>().material.SetFloat("_ClippingPlaneX", 1.5f);
                    o.GetComponent<Renderer>().material.SetFloat("_ClippingPlaneY", -100);
                    o.GetComponent<Renderer>().material.SetFloat("_ClippignPlaneXT", -4);
                }
            }
            
        }
    }
}