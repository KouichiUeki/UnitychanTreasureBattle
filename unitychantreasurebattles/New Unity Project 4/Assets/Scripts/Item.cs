﻿using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
