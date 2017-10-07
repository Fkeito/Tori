using UnityEngine;
using System.Collections;

public class ObjectForBird : VRObjectBase {

    private GameObject bird;

    public bool caught = false;

    void Start() {
        bird = GameObject.FindGameObjectWithTag("Bird");
    }

    void Update() {
        Caught();
    }

    void Caught()
    {
        if (!caught)
        {
            GetComponent<Collider>().isTrigger = false;
            return;
        }
        Debug.Log("Caught");
        GetComponent<Collider>().isTrigger = true;
        transform.position = bird.transform.position -
            new Vector3(0, transform.lossyScale.y * 0.5f, 0);
    }

}
