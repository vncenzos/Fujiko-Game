using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private Transform laserStart;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] public int laserDamage;
    [SerializeField] public float laserKnockback;
    private HealthSystem characterHealth;
    [SerializeField] float[] timeIntervals;
    public bool isActive;
    // Start is called before the first frame update

    enum LaserDirections{ 
        up,
        down,
        left,
        right
    }

    [SerializeField] private LaserDirections laserDirection;
    private Vector3 directionV;
    void Start()
    {
        StartCoroutine(laserPattern());
    }

    // Update is called once per frame
    void Update()
    {
        switch(laserDirection){
            case LaserDirections.up:
            directionV = Vector3.up;
            break;
            case LaserDirections.down:
            directionV = Vector3.down;
            break;            
            case LaserDirections.left:
            directionV = Vector3.left;
            break;            
            case LaserDirections.right:
            directionV = Vector3.right;
            break;
        }
        if (isActive)
        {
            if (Physics.Raycast(laserStart.position, directionV, out RaycastHit laserEnd, 100, layerMask))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, laserStart.position);
                lineRenderer.SetPosition(1, laserEnd.point);
                if (laserEnd.transform.tag == "Player")
                {
                    characterHealth = laserEnd.transform.GetComponent<HealthSystem>();
                    characterHealth.onDamage(laserDamage);
                    characterHealth.onKnockback(laserKnockback);
                }
                else
                {
                    characterHealth = null;
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    IEnumerator laserPattern()
    {
        isActive = !isActive;
        yield return new WaitForSeconds(timeIntervals[0]);
        isActive = !isActive;
        yield return new WaitForSeconds(timeIntervals[1]);
        StartCoroutine(laserPattern());
    }
}
