using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshTherapist : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    public Transform startPos;
    public Transform vinePos;

    public Transform currentTransform;

    therapistDialogues therapistDialoguesScript;

    public bool reached, therapistAtMine, therapistNearPlayer;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //player = GameObject.Find("Player").GetComponent<Transform>();

        //agent.SetDestination(player.position);
        InvokeRepeating("updateTarget", 0.5f, 1.2f);

        currentTransform = player;

        therapistDialoguesScript = GetComponent<therapistDialogues>();

        therapistNearPlayer=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude != 0f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (Vector3.Magnitude(agent.velocity) == 0f)
        {
            reached = true;
            LookAtPlayer();
        }
        else
        {
            reached = false;
        }

        // Check if vinePos is assigned before using it
        if (vinePos != null)
        {
            if (currentTransform == vinePos && agent.velocity.magnitude == 0f)
            {
                therapistAtMine = true;
            }
            else
            {
                therapistAtMine = false;
            }
        }


        if (Vector3.Distance(this.transform.position, player.position) <= 5f)
        {
            therapistNearPlayer = true;
        }

        else
        {
            therapistNearPlayer = false;

        }

    }

    private void OnAnimatorMove()
    {
        if (anim.GetBool("isWalking"))
        {
            agent.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        }
    }

    void updateTarget()
    {
        Debug.Log("updating target");
        agent.SetDestination(currentTransform.position);
    }

    public void setTransform(int i)
    {
        switch (i)
        {
            case 0:
                currentTransform = player;
                break;

            case 1:
                currentTransform = startPos;
                break;

            case 2:
                currentTransform = vinePos;
                break;
        }
    }

    public void setTransform(int i, bool mainScene)
    {
        currentTransform = startPos;
    }


    void LookAtPlayer()
    {
        transform.LookAt(player.position);
    }
}
