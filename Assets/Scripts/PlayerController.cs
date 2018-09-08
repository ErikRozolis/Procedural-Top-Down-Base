using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public Character characterScript;

    GameController gc;

    private void Start()
    {
        SpriteRenderer[] srList = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in srList)
        {
            sr.sortingLayerName = "Player";
        }
        gc = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0))
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.gameObject.transform.position;
            rb.velocity = dir.normalized * 20;
            characterScript.Animator.SetBool("Run", true);
        }
        if(Input.GetMouseButtonUp(0))
        {
            rb.velocity = Vector2.zero;
            characterScript.Animator.SetBool("Run", false);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyController>()!= null)
        {
            gc.StartFight();
        }
    }
}
