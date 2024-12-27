using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetection : MonoBehaviour
{

    public therapistDialogues therapist;
    public selfDoubtManager selfDoubts;

    public GameObject wormholeStuff;
    public GameObject moveObj;
    public Transform xrOrigin;
    public Transform newXRoriginPos;

    private void OnTriggerStay(Collider other)
    {
        if (this.gameObject.name == "PlayerEnteringSelfDoubt")
        {
            
        }

        else if(this.gameObject.name =="Mine")
        {
            if (other.name == "Player")
            {
                //player reached the mine
                therapist.playerAtMine = true;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.name == "PlayerEnteringSelfDoubt")
        {
            
        }
        else if (this.gameObject.name == "Mine")
        {
            if (other.name == "Player")
            {
                //player reached the mine
                therapist.playerAtMine = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.name== "PlayerEnteringSelfDoubt")
        {
            if (other.name == "Player")
            {
                //player entered the mine
                selfDoubts.StopRepeatingEffects();

                wormholeStuff.SetActive(true);
                moveObj.SetActive(false);
                xrOrigin.position = newXRoriginPos.localPosition;
                /*Vector3 rot = xrOrigin.rotation.eulerAngles;
                rot.y += 90f;
                xrOrigin.rotation = Quaternion.Euler(rot);*/
            }
        }
    }
}
