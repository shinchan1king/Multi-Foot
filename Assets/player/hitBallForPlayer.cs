using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBallForPlayer : MonoBehaviour {


	[Header("Shot Power Information")]
	public float shootspeed = 1000f;
	public float curveShotPowerUp = 400;
	public float powerShotSpeedForward = 250;
	public float powerShotSpeedDown = 250;
	public float lobSpeedUp = 1000;
	public float lobSpeedForward = 500;
	public float curveMin;
	public float curveMax;
	public float lobTorqueUp = 500;
	public float dribbleSpeed = 100;
	[Header("Shot Key Code Info")]
	public KeyCode lobShotKeyCode = KeyCode.V;
	public KeyCode normalShotKeyCode = KeyCode.X;
	public KeyCode curveShotKeyCode = KeyCode.Z;
	public KeyCode powerShotKeyCode = KeyCode.C;
	[Header("References")]
	public GameObject player;
	public GameObject ball;
	public Camera playercamera;
	public Rigidbody rb;
	[Header("Audio")]
	public AudioSource footballSound;
	public AudioSource bounceSound;
	public AudioSource dribbleSound;
	[Header("Bool")]
	public bool isKicked = false;
	public bool addCurve = false;
	public bool addDip = false;

	// Use this for initialization
	void Start () {
		rb = ball.GetComponent<Rigidbody> ();
		player = this.gameObject;


	}




	
	void OnTriggerStay(Collider other) {



	
		if (Input.GetKeyDown(normalShotKeyCode) && other.gameObject.tag == "Ball")  {



			rb.AddForce (playercamera.transform.forward * shootspeed * Time.deltaTime, ForceMode.Impulse);
			footballSound.Play ();
			isKicked = true;
			addDip = true;
			
		}

		if (Input.GetKeyDown(curveShotKeyCode) && other.gameObject.tag == "Ball")  {



			rb.AddForce (playercamera.transform.forward * shootspeed * Time.deltaTime, ForceMode.Impulse);
			rb.AddForce (playercamera.transform.up * curveShotPowerUp * Time.deltaTime, ForceMode.Impulse);
			footballSound.Play ();
			addDip = true;
			addCurve = true;

		}


		if (Input.GetKeyDown(powerShotKeyCode) && other.gameObject.tag == "Ball")  {



			rb.AddForce (-player.transform.up *powerShotSpeedDown * Time.deltaTime, ForceMode.Impulse);
			rb.AddForce (playercamera.transform.forward *powerShotSpeedForward * Time.deltaTime, ForceMode.Impulse);

			footballSound.Play ();
			addDip = true;

		}
		if (Input.GetKeyDown(lobShotKeyCode) && other.gameObject.tag == "Ball")  {



			rb.AddForce (player.transform.up * lobSpeedUp* Time.deltaTime, ForceMode.Impulse);
			rb.AddForce (playercamera.transform.forward *lobSpeedForward * Time.deltaTime, ForceMode.Impulse);
			rb.AddTorque (-player.transform.right * lobTorqueUp * Time.deltaTime, ForceMode.Impulse);
			footballSound.Play ();
			addDip = true;
		}

		}


	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
		
			Application.Quit ();


		
		}

		if (isKicked == true) {   // Curve force added each frame
			
			StartCoroutine(iskickedStopTimer());


		

			
		}

		if (addDip == true) {
		
			StartCoroutine (DipAdd ());
		
		}

		if (addCurve == true) {
		
			StartCoroutine (CurveAdd ());
		}


	}

	void OnCollisionEnter(Collision col) {
	

		//if (col.gameObject.tag == "Ground") {

		//	bounceSound.Play();

		//}

		if (col.gameObject.tag == "Ball") {
		
		
			dribbleSound.Play ();
			rb.AddForce (player.transform.forward * 0+ player.GetComponent<Rigidbody>().velocity * dribbleSpeed, ForceMode.Impulse);
			player.GetComponent<Rigidbody>().AddForce (-player.transform.forward * 100f,  ForceMode.Impulse);
		
		}


	
	}

	IEnumerator iskickedStopTimer() {
		rb.AddForce (-playercamera.transform.right* Random.Range (0.3f, 0.7f), ForceMode.Impulse);
		rb.AddForce (playercamera.transform.right* Random.Range (0.6f, 1f), ForceMode.Impulse);
		rb.AddForce (-playercamera.transform.right* Random.Range (0.4f, 0.8f), ForceMode.Impulse);
		rb.AddForce (player.transform.up * 0.5f, ForceMode.Impulse);
		rb.AddForce (playercamera.transform.right* Random.Range (0.4f, 0.6f), ForceMode.Impulse);
		rb.freezeRotation = true;
		yield return new WaitForSeconds (1.5f);
		rb.freezeRotation = false;
		isKicked = false;
	}

	IEnumerator DipAdd() 
	{
		rb.AddForce (-player.transform.up * 0.1f, ForceMode.Impulse);
		yield return new WaitForSeconds (1.5f);
		addDip = false;
	}

	IEnumerator CurveAdd() {
	
		rb.AddForce (-player.transform.right* Random.Range (curveMin, curveMax) * Time.deltaTime, ForceMode.Impulse);
		yield return new WaitForSeconds (1.5f);
		addCurve = false;
	
	}


		
		 
	
	}


