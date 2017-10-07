using UnityEngine;
using System.Collections;

public class Bird : VRObjectBase {

    public Camera mainCamera;

    private bool VR = false;

    private bool inCage = true;//鳥が籠の中にいるか
    private bool flyFlag = true;//鳥が飛べるか
    private bool isFly = false;//鳥が飛んでいるか
    private bool isFlyBack = false;//鳥が戻ってきてるか

    private GameObject obj;//持ってきたいオブジェクト
    private VRObjectMode objMode;//持ってきたいオブジェクトのモード
    private Vector3 direction;//飛行の中心軸

    void Start()
    {
        if (device != null) VR = true;
        inCage = false;
    }

    void Update()
    {
        if (VR)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger){
               // SetFly(device.transform.pos,)
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) SetFly();
        }
    }

    void Fly(Vector3 startPos)
    {
        transform.position = startPos + direction;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * 5;

        isFly = true;
        flyFlag = false;
    }
    void FlyBack()
    {
        transform.LookAt(mainCamera.transform.position);

        direction = (mainCamera.transform.position - transform.position).normalized;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * 5;
        isFlyBack = true;

        Invoke("FinishFly", 3f);
    }

    void SetFly(Ray ray)
    {
        Debug.Log("Set");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit");

            transform.LookAt(hit.point);
            rigidBody.useGravity = false;

            direction = (hit.point - ray.origin).normalized;
            Debug.Log(GetVRObjectMode());
            SetVRObjectMode(VRObjectMode.None);
            Debug.Log(GetVRObjectMode());

            Fly(ray.origin);
        }
    }
    void SetFly()
    {
        if (!flyFlag || inCage) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        SetFly(ray);
    }
    void SetFly(Vector3 startPos, Vector3 direction)
    {
        if (!flyFlag || inCage) return;

        Ray ray = new Ray(startPos, direction);
        SetFly(ray);
    }

    void FinishFly()
    {
        if (!isFlyBack) return;

        if (obj)
        {
            obj.gameObject.GetComponent<ObjectForBird>().caught = false;
            obj.transform.parent = null;
        }
        rigidBody.velocity *= 0.5f;
        rigidBody.useGravity = true;

        obj = null;
        objMode = VRObjectMode.None;
        direction = Vector3.zero;
    }

    void OnCollisionEnter(Collision other)
    {
        if (isFly)
        {
            var tmpObj = other.gameObject.GetComponent<VRObjectBase>();
            if (tmpObj)
            {
                if (tmpObj.GetVRObjectMode() != VRObjectMode.NeverMove)
                {
                    Debug.Log("Catch");
                    objMode = tmpObj.GetVRObjectMode();
                    //tmpObj.SetVRObjectMode(VRObjectMode.None);
                    obj = other.transform.gameObject;
                    obj.GetComponent<ObjectForBird>().caught = true;
                    obj.transform.parent = this.transform;
                }
            }
            SetVRObjectMode(VRObjectMode.Grabable);

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            Invoke("FlyBack", 0.5f);
        }

        isFly = false;
        //isFlyBack = false;
        flyFlag = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (isFlyBack)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FinishFly();
                isFlyBack = false;
            }
        }
    }
}
