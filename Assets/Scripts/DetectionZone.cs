using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider2D col;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collison) {
        //add the collider to the list of detected colliders
        detectedColliders.Add(collison);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //remove the collider from the list of detected colliders
        detectedColliders.Remove(collision);
    }
}
