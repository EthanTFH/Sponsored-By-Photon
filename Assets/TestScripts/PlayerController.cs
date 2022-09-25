using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace net.EthanTFH.BTSGameJam
{
    public class PlayerController : MonoBehaviourPun
    {
        [SerializeField]
        [field: Tooltip("Player NameTag GameObject")]
            private GameObject nameTag;

        [SerializeField]
        [field: Tooltip("The Local Player Instance, can be accessed by other scripts.")]
            public static GameObject LocalPlayerInstance;

        [field: Header("Player Variables")]
        [field: SerializeField, Tooltip("Player Turn Speed"), Range(5.0f, 30.0f)] protected float turnSpeed { get; private set; }
        [field: SerializeField, Tooltip("Player Walk Speed"), Range(3.0f, 15.0f)] protected float walkSpeed { get; private set; }

        void Awake()
        {
            if (photonView.IsMine)
                PlayerController.LocalPlayerInstance = this.gameObject;

            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Ensure we have a name tag object.
            if (nameTag == null)
                nameTag = this.transform.Find("NameTag").gameObject;

            // Set the name tag of the cube.
            nameTag.GetComponent<TMPro.TextMeshPro>().text = photonView.Owner.NickName;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            // Ensures that we are controlling only our own player.
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            // Detects what view (and the movement method) we should use
            if ((PhotonNetwork.IsMasterClient && Camera.main.GetComponent<CameraModeOne>().SmallPlayer == CameraModeOne.ClientType.MasterClient) ||
                (!PhotonNetwork.IsMasterClient && Camera.main.GetComponent<CameraModeOne>().SmallPlayer == CameraModeOne.ClientType.NormalClient))
                MovementTwo();
            else
                MovementThree();

            // Code to make *all* name tags face the camera.
            GameObject[] nameTags = GameObject.FindGameObjectsWithTag("NameTag");
            for (int i = 0; i < nameTags.Length; i++)
                nameTags[i].transform.LookAt(Camera.main.transform);

        }

        //Function for 2D Movement
        private void MovementTwo()
        {
            // Code for player movement in the 2nd Dimension.
            if (Input.GetKey(KeyCode.A))
                transform.position = transform.position - transform.forward * walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                transform.position = transform.position + transform.forward * walkSpeed * Time.deltaTime;
        }


        //Function for 3D Movement
        private void MovementThree()
        {
            // Code for player movement in the 3rd Dimension.
            if (Input.GetKey(KeyCode.W))
                transform.position = transform.position + transform.forward * walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                transform.position = transform.position - transform.forward * walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.down * turnSpeed * 10 * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.up * turnSpeed * 10 * Time.deltaTime);
        }

#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }

        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }

        void CalledOnLevelWasLoaded(int level)
        {
            // Ensure the player is actually on top of something.
            if(!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                if (GameObject.Find("Floor"))
                    transform.position = new Vector3(0f, GameObject.Find("Floor").transform.position.y + 5, 0f);
                else
                    transform.position = new Vector3(0f, 5f, 0f);
            }
        }
#endif
    }
}