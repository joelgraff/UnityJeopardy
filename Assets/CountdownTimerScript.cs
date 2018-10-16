using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimerScript : MonoBehaviour {

	private UnityEngine.UI.Text timerPanel;

	private int seconds = 60;

	// Use this for initialization
	void Start () {
		
		timerPanel = GetComponentInChildren<UnityEngine.UI.Text>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startTimer() {

		timerPanel.transform.localScale=Vector3.one;
		InvokeRepeating("updateTimer", 0.0f, 1.0f);

	}

	private void updateTimer() {

		if (seconds < 0)
		{
			stopTimer();
			return;
		}

		string textTime = seconds.ToString();
		
		if (seconds < 10)
			textTime = "0" + textTime;

		timerPanel.text = "0:" + textTime;
		seconds--;
	}

	public void stopTimer() {

		CancelInvoke();
		seconds = 60;
		timerPanel.transform.localScale = Vector3.zero;

	}

	public bool hasTimedOut() {

		return seconds < 0;

	}
}
