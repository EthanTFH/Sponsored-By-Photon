using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerMoving;
    [HideInInspector]
    public bool isMoving = false;
    [HideInInspector]
    private Transform originalParent;

    // Start is called before the first frame update
    void Start()
    {
        originalParent = gameObject.transform.parent;
    }



    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            gameObject.transform.position = playerMoving.transform.position + playerMoving.transform.forward * 4;
            if(gameObject.transform.parent != playerMoving.transform)
                gameObject.transform.SetParent(playerMoving.transform);
        }else{
            gameObject.transform.SetParent(originalParent);
            playerMoving = null;
        }
    }
}
