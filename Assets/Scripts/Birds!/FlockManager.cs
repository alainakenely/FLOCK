using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public List<GameObject> flockBirds = new List<GameObject>();

    public void AddToFlock(GameObject newBird)
    {
        if (!flockBirds.Contains(newBird))
        {
            flockBirds.Add(newBird);
            Debug.Log("🪶 Added " + newBird.name + " to the flock!");
        }
    }
}