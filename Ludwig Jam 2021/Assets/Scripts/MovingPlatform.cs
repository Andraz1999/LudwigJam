using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingPlatform : MonoBehaviour
{
    //public int triggerIntLimit;
    [Header("Moving Platform")]
    //public GameObject allThePoints;
    [SerializeField] Transform[] points;
    private Transform point;
    private bool isPointSet;

    [SerializeField] bool isEnabled;
    public float movingSpeed;
    //public bool waitingForPlayer;
    public float pauseFor = 2f;

    //private Rigidbody2D rb;
    [SerializeField] GameObject particles;

    [SerializeField] int conditions = 1;
    private int currentConditions;

    private AudioManager audioManager;
    

    private void Start() 
    {
        audioManager = AudioManager.Instance;
        //points = allThePoints.GetComponentsInChildren<Transform>();
        if(points.Length != 0)
        point = points[0];  
        //rb = GetComponent<Rigidbody2D>(); 
        if(conditions == 0)
            EnablePlatform();
        else
            DisablePlatform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isEnabled)
        {
            if (isPointSet)
            Move();
            else
            {  
                StartCoroutine("GetNextPoint", point);
            } 
        }
    }

    void Move()
    {
        Vector3 dir = (point.position - transform.position).normalized;
        //rb.velocity = (dir * movingSpeed * Time.deltaTime);
        transform.position = transform.position + (dir * movingSpeed * Time.deltaTime);
        if((point.position - transform.position).magnitude < 0.1f)
        {
            isPointSet = false;
        }  
    }

    IEnumerator GetNextPoint(Transform cur)
    {
        yield return new WaitForSeconds(pauseFor);
        int n = points.Length;
        int i = Array.IndexOf(points, cur);
        isPointSet = true;
        if (n-1 == i) 
        {
            point = points[0];
        }
        else
        { 
            point = points[i+1];
        }
    }
    public void IncreaseCondition(int n)
    {
        currentConditions += n;
        if (currentConditions >= conditions)
        {
            audioManager.Play("open");
            EnablePlatform();
        }
    }
    void EnablePlatform()
    {
        isEnabled = true;
        particles.SetActive(true);
    }
    void DisablePlatform()
    {
        isEnabled = false;
        particles.SetActive(false);
    }

    // public void Respawn()
    // {
    //     DisablePlatform();
    //     if(conditions == 0)
    //         EnablePlatform();
    //     transform.position = points[0].position;
    // }

        private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {

            //waitingForPlayer = false;
            other.transform.parent = this.transform;
        }    
    }

    
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            other.transform.parent = null;
        }  
    }
}
