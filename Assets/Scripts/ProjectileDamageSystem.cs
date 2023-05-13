using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageSystem : MonoBehaviour
{

    public int damage;
    public float timeToLife;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BotScript>().TakeDamage(damage);
        }
    }

    private void Update()
    {
        Invoke("Destory", timeToLife);
    }

    private void Destory()
    {
        Destroy(this.gameObject);
    }
}
