﻿//This script is based on the following tutorial:
//https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/fun-with-lasers

using UnityEngine;
using System.Collections;

public class LaserCharge : MonoBehaviour {

	private InputControl input;
	LineRenderer line;
	public int length = 100;
	public int damage = 5;
	public int force = 500;
    public int dMultiplier;
	public bool shouldFire = true;
	private God god;
    public GameObject intensityText;

    void Start () {
		input = GameObject.FindWithTag ("InputControl").GetComponent<InputControl> ();
		god = GameObject.FindWithTag ("GameController").GetComponent<God> ();
        intensityText = GameObject.Find("Intensity");
        line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	void FixedUpdate () {
        if (shouldFire)
        {
            //Debug.Log ("Fire!");
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");

            dMultiplier = (int) god.intMultiplier;

        }
        else {  }

    }

    IEnumerator FireLaser(){
        //line.enabled = true;
        GetComponent<ParticleSystem>().maxParticles = 0;

        //Player's beam
        while ((input.RH > 0.2f || input.RV > 0.2 || input.RH < -0.2 || input.RV < -0.2) && shouldFire && this.tag == "PlayerBeam") {
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;

            GetComponent<ParticleSystem>().maxParticles = 500;

            line.SetPosition(0, ray.origin);
			
			//Checks if anything blocks the laser
			if(Physics.Raycast (ray, out hit, length)){
				line.SetPosition(1, hit.point);
				if(hit.collider.tag == "Enemy"){
					//Debug.Log ("Hit!");
					switch (hit.collider.name){
					case "EnemyDrone(Clone)":
						//Debug.Log ("Drone Hit");
						hit.collider.gameObject.GetComponent<EnemyDrone>().charge+=damage*2+damage*dMultiplier;
						break;
					case "EnemyWall(Clone)":
						//Debug.Log ("Wall Hit");
						hit.collider.gameObject.GetComponent<EnemyWall>().charge += damage * 2 + damage * dMultiplier;
                            break;
					case "EnemyHugger(Clone)":
						//Debug.Log ("Hugger Hit");
						hit.collider.gameObject.GetComponent<EnemyHugger>().charge += damage * 2 + damage * dMultiplier;
                            break;
					case "Bolt(Clone)":
						//Debug.Log ("Bolt Hit");
						hit.collider.gameObject.GetComponent<EnemyBolt>().charge += damage * 2 + damage * dMultiplier;
                            break;
					}

					//hit.collider.gameObject.GetComponent<EnemyFighter>().charge+=damage;

				}
			}
			else{
				//Shoots laser whole duration if nothing blocks it.
				//If laser should go through objects, use only this line
				line.SetPosition(1, ray.GetPoint(length));
			}


			yield return null;
		}
		//Enemy's beam
		while (shouldFire && this.tag == "EnemyBeam") {
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			line.SetPosition(0, ray.origin);
			
			//Checks if anything blocks the laser
			if(Physics.Raycast (ray, out hit, length)){
				if ((hit.collider.tag == "PlayerShip" || hit.collider.tag == "PlayerShield") && hit.rigidbody) {
					//line.SetPosition(1, hit.point);
					//Debug.Log ("Boom!");
					hit.rigidbody.AddForce (-transform.right * force);
                    hit.transform.GetComponent<PlayerControl>().shieldPower -= damage;
                    god.shield = hit.transform.GetComponent<PlayerControl>().shieldPower;
                    intensityText.GetComponent<Intensity>().AddIntensity(-damage * god.intMultiplier);

                    line.SetPosition (1, ray.GetPoint (length));
				} else {
					line.SetPosition(1, ray.GetPoint(length));
				}
			}
			else{
				//Shoots laser whole duration if nothing blocks it.
				//If laser should go through objects, use only this line
				line.SetPosition(1, ray.GetPoint(length));
			}
			yield return null;
		}
		line.enabled = false;
	}
}
