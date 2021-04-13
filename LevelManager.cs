using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        Instantiate(player, transform.position, transform.rotation);
    }
}
