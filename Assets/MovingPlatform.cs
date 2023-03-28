using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] public Transform Point1;
    [SerializeField] public Transform Point2;
    public float platformSpeed;
    public float platformTime;
    private Vector3 point1V;
    private Vector3 point2V;
    private Vector3 target;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        target = point2V;
        point1V = Point1.position;
        point2V = Point2.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > platformTime)
        {
            timer = 0f;
            if(target == point2V)
            {
                target = point1V;
            }
            else
            {
                target = point2V;
            }
            Vector3 force = target - transform.position;
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
      {
          other.transform.SetParent(this.transform);
      }
  }

  private void OnTriggerExit(Collider other)
  {
      if (other.tag == "Player")
      {
          other.transform.SetParent(null);
      }
  }
}
