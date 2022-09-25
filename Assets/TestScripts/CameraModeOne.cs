using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace net.EthanTFH.BTSGameJam {
    public class CameraModeOne : MonoBehaviour
    {
        [field: Header("Camera Settings")]
        [field: SerializeField, Range(5, 20)]
        protected float cameraMaxDistance = 10f;
        [field: SerializeField, Range(3, 15)]
        protected float cameraMinDistance = 3f;
        public float mouseSensitivity = 3.0f;
        [field: Range(-100, 100)]
        public float maxX = 40, minX = -40;


        [SerializeField]
        private Vector3 smoothVelocity = Vector3.zero;
        [SerializeField]
        private float smoothTime = 0.2f;
        [SerializeField]
        private bool doCaptureMouse = true;

        [field: Header("Objects"), SerializeField]
        private Camera cam;

        public enum ClientType
        {
            MasterClient = 0,NormalClient = 1
        }

        public ClientType SmallPlayer = ClientType.NormalClient;

        private Transform targetPlayer;
        private float distanceFromTarget = 4.0f;
        private float rotationY, rotationX;
        private Vector3 currentRotation;

        private bool movingObject = false;
        private GameObject objectBeingMoved = null;
        
        void Start()
        {
            // If We are the Small Player we want to disable 3D view and enable 2D view
            if ((SmallPlayer == ClientType.MasterClient && PhotonNetwork.IsMasterClient) ||
                (SmallPlayer == ClientType.NormalClient && !PhotonNetwork.IsMasterClient))
            {
                gameObject.GetComponent<CameraModeTwo>().enabled = true;
                gameObject.GetComponent<CameraModeOne>().enabled = false;
            }
            

            // Capture the cursor
            if (doCaptureMouse)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.None;

            // Obtain the camera and target player
            if (cam == null)
                cam = Camera.main;
            if (targetPlayer == null && PlayerController.LocalPlayerInstance != null)
                targetPlayer = PlayerController.LocalPlayerInstance.transform;


            
        }

        void FixedUpdate()
        {
            if (targetPlayer == null || targetPlayer.transform == null)
            {
                if(PlayerController.LocalPlayerInstance)
                    targetPlayer = PlayerController.LocalPlayerInstance.transform;
                return;
            }

            float mouseX = 0f, mouseY = 0f;

            if (Input.GetMouseButton(1))
            {
                // Update Y Rotation around player.
                mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(!movingObject)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo))
                    {
                        if (hitInfo.collider.gameObject.CompareTag("Moveable"))
                        {
                            hitInfo.collider.gameObject.GetComponent<MoverScript>().playerMoving = targetPlayer.gameObject;
                            hitInfo.collider.gameObject.GetComponent<MoverScript>().isMoving = true;
                            movingObject = true;
                            objectBeingMoved = hitInfo.collider.gameObject;
                        }
                    }
                }
                else
                {
                    objectBeingMoved.GetComponent<MoverScript>().isMoving = false;
                    movingObject = false;
                    objectBeingMoved = null;
                }
            }

            distanceFromTarget -= Input.mouseScrollDelta.y;
            distanceFromTarget = Mathf.Clamp(distanceFromTarget, 3.0f, 10.0f);

            rotationY += mouseX;
            rotationX += mouseY;

            rotationX = Mathf.Clamp(rotationX, minX, maxX);

            Vector3 nextRotation = new Vector3(rotationX, rotationY);
            currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
            transform.localEulerAngles = currentRotation;

            transform.position = targetPlayer.position - transform.forward * distanceFromTarget;
        }
    }
}