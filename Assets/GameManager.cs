using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform player;

    public GameObject playerObj;
    public TMP_Text speed;
    public TMP_Text state;
    public TMP_Text deathScreen;
    public GameObject cameraFPS;


    public float speedCount;
    public string stateCount;

    public bool playerDead;
    

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        playerDead = false;

        deathScreen.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        speed.text = "Speed: " + rb.velocity.magnitude;
        state.text =  player.GetComponent<PlayerMovement>().state.ToString();

        if (playerDead)
        {
            endGame();
        }
    }

    private void endGame()
    {
        playerObj.GetComponent<PlayerMovement>().enabled = false;
        playerObj.GetComponent<GrappleHook>().enabled = false;
        playerObj.GetComponent<Dash>().enabled = false;

        deathScreen.enabled = true;
    }


}
