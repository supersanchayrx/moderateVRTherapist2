using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetection : MonoBehaviour
{

    public therapistDialogues therapist;
    public selfDoubtManager selfDoubts;

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
            }
        }
    }
}
