using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Camera cam;
	public GameObject[] balls;
	public float timeLeft;
	public Text timerText;
	public GameObject gameOverText;
	public GameObject restartButton;
	public GameObject splashScreen;
	public GameObject startButton;
	public HatController hat_Controller;

	private float maxWidth;
	private bool playing;
	private int ballCount;

	// Use this for initialization
	void Start () {

		if (cam == null) {
			cam = Camera.main;
		}
		playing = false;
		Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
		Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);
		float ballWidth = balls[0].GetComponent<Renderer>().bounds.extents.x;
		maxWidth = targetWidth.x-ballWidth;
		//StartCoroutine (Spawn ());
		UpdateText ();
	}

	void FixedUpdate () {
		if (playing) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				timeLeft = 0;
			}
			UpdateText ();
		}
	}

	public void StartGame () {
		splashScreen.SetActive (false);
		startButton.SetActive (false);
		playing = true;
		hat_Controller.toggledControl (true);
		StartCoroutine (Spawn ());
	}

	public void BallCountUpdate () {
		ballCount--;
	}

	IEnumerator Spawn () {
		//yield return new WaitForSeconds (2.0f);
		while (timeLeft > 0) {
			int rand = Random.Range (1, 4);
			while(rand>0){
				GameObject ball = balls[Random.Range (0, balls.Length)];
				Vector3 spawnPosition = new Vector3 (
					Random.Range (-maxWidth, maxWidth), 
					transform.position.y, 
					0.0f
				);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (ball, spawnPosition, spawnRotation);
				ballCount++;
				rand--;
				yield return new WaitForSeconds (Random.Range (0.5f, 0.7f));
			}
			yield return new WaitForSeconds (Random.Range (1.0f, 2.0f)); //Wait for 1 or 2 seconds & go for the loop again
		}
		yield return new WaitForSeconds(2.0f);
		gameOverText.SetActive (true);
		yield return new WaitForSeconds(1.0f);
		restartButton.SetActive (true);
	} 

	void UpdateText () {
		timerText.text = "Time Left\n" + Mathf.RoundToInt (timeLeft);
	}
}
