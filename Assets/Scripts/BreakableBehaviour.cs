using UnityEngine;
using System;

public class BreakableBehaviour : MonoBehaviour
{
    [SerializeField]
    AudioClip breakSound; // Sound to play when the object breaks
    public event Action OnBreak; // Event to notify when the object breaks
    public void Break()
    {
        Debug.Log(gameObject.name + " broken!");

        OnBreak?.Invoke(); // Invoke the break event if there are any subscribers
        AudioSource.PlayClipAtPoint(breakSound, transform.position); // Play the break sound
        Destroy(gameObject);
    }
}
