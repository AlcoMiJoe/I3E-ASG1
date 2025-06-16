using UnityEngine;

public class BreakableBehaviour : MonoBehaviour
{
    public void Break()
    {
        Debug.Log(gameObject.name + " broken!");
        Destroy(gameObject);
    }
}
