using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceRoll : MonoBehaviour
{
    public GameObject character;
    public static int DiceNumber;
    Vector3 MOVEX = new Vector3(0.64f, 0, 0);

    float step = 1000f;
    Vector3 target;
    Vector3 prevPos;

    private void Start()
    {
        character = GameObject.Find("UTC_Default");
        target = character.transform.position;
    }

    public void OnClic()
    {
        DiceNumber = Random.Range(1, 7);

        MOVEX.x = DiceNumber;
        Debug.Log(MOVEX);

        prevPos = target;
        target += MOVEX;

        Move();
    }

    // Update is called once per frame
    void Update()
    {

        // ① 移動中かどうかの判定。移動中でなければ入力を受付
        /*
        if (transform.position == target)
        {
            return;
        }
        */
        Move();
    }

    void Move()
    {
        character.transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

}
