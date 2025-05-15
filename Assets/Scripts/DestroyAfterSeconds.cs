using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
