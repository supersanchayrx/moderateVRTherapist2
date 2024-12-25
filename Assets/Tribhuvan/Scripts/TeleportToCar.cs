using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToCar : MonoBehaviour
{
    public Transform player;
    public Transform carSeatPosition;
    public FadeManager fadeManager;
    public GameObject moveObj;
    public ParallaxEffect pe;

    public GameObject stepHereCanvas, therapist;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            StartCoroutine(TeleportSequence());
        }
    }

    private IEnumerator TeleportSequence()
    {
        moveObj.SetActive(false);
        stepHereCanvas.SetActive(false);
        therapist.SetActive(false);
        fadeManager.FadeOut();
        yield return new WaitForSeconds(1.5f); // Wait for fade out animation
        player.position = carSeatPosition.localPosition;
        Vector3 rot = player.rotation.eulerAngles;
        rot.y += 90f;
        player.rotation = Quaternion.Euler(rot);
        yield return new WaitForSeconds(1f); // Wait before fading in
        fadeManager.FadeIn();
        pe.StartParallax();
    }
}
