using UnityEngine;


public class BirdMove : MonoBehaviour
{
    public Transform splingshot;
    public float splingRange;
    public float maxVel;


    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    bool canDrag = true;
    Vector3 dis;

     void OnMouseDrag()
    {
        if (!canDrag)
            return;

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dis = pos - splingshot.position;
        dis.z = 0;

        if (dis.magnitude > splingRange)
        {
            dis = dis.normalized * splingRange;
        }
        transform.position = dis + splingshot.position;
    }

    void OnMouseUp()
    {
        if (!canDrag)
            return;
        canDrag = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = -dis.normalized * maxVel * dis.magnitude / splingRange;
    }


}
