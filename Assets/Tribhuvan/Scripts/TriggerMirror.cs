using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMirror : MonoBehaviour
{
    public FadeManager fm;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            StartCoroutine(TeleportSequence());
        }
    }

    private IEnumerator TeleportSequence()
    {
        fm.FadeOut();
        yield return new WaitForSeconds(1.5f); // Wait for fade out animation
        yield return new WaitForSeconds(1f); // Wait before fading in
        fm.FadeIn();
    }
}
