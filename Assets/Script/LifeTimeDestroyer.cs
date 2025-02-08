using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float Time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, Time);
    }
}
