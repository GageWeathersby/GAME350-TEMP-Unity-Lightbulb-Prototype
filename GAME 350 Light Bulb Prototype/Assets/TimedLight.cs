using UnityEngine;
using System.Collections;

public class TimedLight : MonoBehaviour
{
    private Light myLight;
    private GameObject playerInRange; // Tracks if a player is currently inside

    [Header("Timer Settings")]
    public float delayInSeconds = 3.0f;

    [Header("Target Settings")]
    public string playerTag = "Player";

    void Start()
    {
        myLight = GetComponent<Light>();
        StartCoroutine(TurnOffAndCheckPlayer());
    }

    IEnumerator TurnOffAndCheckPlayer()
    {
        myLight.enabled = true;
        yield return new WaitForSeconds(delayInSeconds);
        
        // 1. Turn off the light
        myLight.enabled = false;

        // 2. Catch the player if they are already standing inside when it goes dark
        if (playerInRange != null)
        {
            DestroyPlayer(playerInRange);
        }
    }

    // Handles a player entering the zone (either before or after the light turns off)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = other.gameObject;

            // If the light is already off, destroy them instantly upon entry
            if (!myLight.enabled)
            {
                DestroyPlayer(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = null;
        }
    }

    // Helper method to execute player destruction safely
    private void DestroyPlayer(GameObject player)
    {
        Destroy(player);
        playerInRange = null; // Clear reference
        Debug.Log("Player stepped into the dark and was destroyed!");
    }
}