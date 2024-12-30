using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

//TODO: Make it so that the characters can still jump as they swap between each other OR make it so that the characters can only jump when they are on the ground
public class CharacterSwap : MonoBehaviour
{
    //used to check if the character is on the ground for the Swap method
    TouchingDirections touchingDirections;
    Vector3 currentLocation;

    //get the character GameObjects
    //Tanjiro, Zenitsu, and Inosuke are the characters we want to swap between
    public GameObject Tanjiro;
    public GameObject Zenitsu;
    public GameObject Inosuke;

    //get the location of the active character
    public Vector3 GetLocation { 
        get
        {
            //get the position of the active character
            if (Tanjiro.activeSelf) {
                return Tanjiro.transform.position;
            } else if (Zenitsu.activeSelf) {
                return Zenitsu.transform.position;
            } else {
                return Inosuke.transform.position;
            }
        } 
        private set {} 
    }

    void Awake()
    {
        //disable all characters except for Tanjiro (our default character)
        Tanjiro.SetActive(true);
        Zenitsu.SetActive(false);
        Inosuke.SetActive(false);
    }

    //TODO: Figure out why this cannot be assigned to the SwappableCharacters prefab (needs to be manually assigned)
    public void OnSwap(InputAction.CallbackContext context) {
        //if the button is pressed
        if (context.started) {
            Debug.Log("Swap");

            //get the location of the active character
            currentLocation = GetLocation;

            //swap the characters. Tanjiro -> Zenitsu -> Inosuke -> Tanjiro
            if (Tanjiro.activeSelf) {
                Swap(Tanjiro, Zenitsu, currentLocation);
            } else if (Zenitsu.activeSelf) {
                Swap(Zenitsu, Inosuke, currentLocation);
            } else if (Inosuke.activeSelf) {
                Swap(Inosuke, Tanjiro, currentLocation);
            }
        }  
    }

    //method to swap between characters
    private void Swap(GameObject currentChar, GameObject nextChar, Vector3 currentLocation)
    {
        //only swap if the character is on the ground
        touchingDirections = currentChar.GetComponent<TouchingDirections>();

        if (touchingDirections.IsGrounded) {
            currentChar.SetActive(false);
            nextChar.transform.position = new Vector3(currentLocation.x, currentLocation.y, currentLocation.z);
            nextChar.SetActive(true);
        }
    }
}
