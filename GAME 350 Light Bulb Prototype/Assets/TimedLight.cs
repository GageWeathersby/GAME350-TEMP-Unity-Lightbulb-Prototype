using System.Collections;
using TMPro;
using UnityEngine;

public class TimedLight : MonoBehaviour
{
    public TextMeshProUGUI gameOverText; // Drag TMP Text here in the Inspector to display it.
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
            if (gameOverText != null)
            {
                gameOverText.gameObject.SetActive(true); // Show the text
            }
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
                if (gameOverText != null)
                {
                    gameOverText.gameObject.SetActive(true); // Show the text
                }

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

    // Additional method to execute player destruction safely. May be redundant, but doesn't break anyting.
    private void DestroyPlayer(GameObject player)
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true); // Show the text
        }
        Destroy(player);
        playerInRange = null; // Clear reference
        Debug.Log("The darkness caught up with you, and you were consumed.");
    }
}