using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPreview : MonoBehaviour
{
    public static ShopPreview instance;
    public float rotateSpeed = 1.0f;
    Quaternion origRot;
    GameObject preview;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        origRot = transform.rotation;
    }

    public void SetPreview(GameObject previewObject)
    {
        if (preview != null) Destroy(preview);
        transform.rotation = origRot;
        preview = Instantiate(previewObject);
        preview.transform.SetParent(transform);
        preview.transform.position = transform.position;

        preview.layer = gameObject.layer;
        for(int i = 0; i < preview.transform.childCount; i++)
        {
            preview.transform.GetChild(i).gameObject.layer = gameObject.layer;
        }
        preview.transform.Rotate(origRot.eulerAngles);
        //preview.lay
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
    }
}
