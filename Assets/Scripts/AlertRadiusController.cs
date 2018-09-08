using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertRadiusController : MonoBehaviour {

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("PriestCharacter");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            gameObject.transform.parent.GetComponent<EnemyController>().Alert();
        }
    }
}
