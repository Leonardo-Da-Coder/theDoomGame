using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform player;

    public TMP_Text speed;
    public TMP_Text state;

    public float speedCount;
    public string stateCount;
    

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed.text = "Speed: " + rb.velocity.magnitude;
        state.text =  player.GetComponent<PlayerMovement>().state.ToString();
    }

    
}
