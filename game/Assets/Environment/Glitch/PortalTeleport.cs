﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour {
    
    public Transform player;
    public Transform reciever;

    private bool playerIsOverlapping = false;

    void Update() {
        if (playerIsOverlapping) {
        	Vector3 portalToPlayer = player.position - transform.position;
        	float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

        	if (dotProduct < 0f) {
        		float rotationDifference = Quaternion.Angle(transform.rotation, reciever.rotation);
        		rotationDifference += 180;
        		player.Rotate(Vector3.up, rotationDifference);

        		Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
        		player.position = reciever.position + positionOffset;
        		playerIsOverlapping = false;
        	}
        }
    }

    void OnTriggerEnter(Collider other) {
    	if (other.tag == "Player") {
    		playerIsOverlapping = true;
    	}
    }

    void OnTriggerExit (Collider other) {
    	if (other.tag == "Player") {
    		playerIsOverlapping = false;
    	}
    }
}