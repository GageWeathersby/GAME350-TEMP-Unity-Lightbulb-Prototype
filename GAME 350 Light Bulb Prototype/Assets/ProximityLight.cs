using UnityEngine;

public class ProximityLight : MonoBehaviour
{
    // Drag your parent Light GameObject into this field in the Inspector
    [SerializeField] private Light targetLight; 
    [SerializeField] private string playerTag = "Player";

    void Start()
    {
        // Fallback: If you forgot to assign it, look for it on the parent object
        if (targetLight == null)
        {
            targetLight = GetComponentInParent<Light>();
        }

        if (targetLight != null)
        {
            targetLight.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && targetLight != null)
        {
            targetLight.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && targetLight != null)
        {
            targetLight.enabled = false;
        }
    }
}