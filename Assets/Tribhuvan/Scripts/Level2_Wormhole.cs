using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2_Wormhole : MonoBehaviour
{
    public GameObject wormholeStuff;
    public GameObject moveObj;
    public Transform xrOrigin;
    public Transform newXRoriginPos;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        wormholeStuff.SetActive(true);
        moveObj.SetActive(false);
        xrOrigin.position = newXRoriginPos.localPosition;
        Vector3 rot = xrOrigin.rotation.eulerAngles;
        rot.y += 90f;
        xrOrigin.rotation = Quaternion.Euler(rot);
    }
}
