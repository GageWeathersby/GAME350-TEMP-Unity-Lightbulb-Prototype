using UnityEngine;
using System.Collections;

public class TimedLight : MonoBehaviour
{
    private Light myLight;
    private GameObject playerInRange; // Tracks if a player is currently inside the light's area.

    [Header("Timer Settings")]
    public float timeUntilDark = 3.0f;
    public Light lightToDim;

    //Checks for what thing(s) to destroy, based on tags.
    [Header("Target Settings")]
    public string playerTag = "Player";

    void Start()
    {
        myLight = GetComponent<Light>();
        //Coroutine to shut off the light and check if player is in its area.
        StartCoroutine(TurnOffAndCheckPlayer());
        //Coroutine to dim the lights over time.
        StartCoroutine(FadeOutLight(timeUntilDark));
    }

    IEnumerator TurnOffAndCheckPlayer()
    {
        myLight.enabled = true;
        yield return new WaitForSeconds(timeUntilDark);
        
        // 1. Turn off the light.
        myLight.enabled = false;

        // 2. Catch the player if they are already standing inside when it goes dark.
        if (playerInRange != null)
        {
            DestroyPlayer(playerInRange);
        }
    }

    IEnumerator FadeOutLight(float time)
    {
        float startIntensity = lightToDim.intensity;
        float elapsed = 0.0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            lightToDim.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / time);
            yield return null;
        }

        lightToDim.intensity = 0f;
    }

    // Handles a tagged object that enters the zone (either before or after the light turns off).
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = other.gameObject;

            // If the light is already off, destroy player instantly upon entry. NOTE this will always kill the player, not any tagged object.
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
        Debug.Log("The darkness caught up with you, and you were consumed.");
    }
}