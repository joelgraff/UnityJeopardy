using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueDisplay : MonoBehaviour {

	private UnityEngine.UI.Text[] answerPanelText;

	private UnityEngine.CanvasGroup canvasGroup;

	private UnityEngine.RectTransform canvasTransform;

	private CountdownTimerScript countdownScript;

	private bool isShowingQuestion = false;

	public string question;
	public string answer;

	// Use this for initialization
	void Start () {

		//get reference to the answer panel's text element
		answerPanelText = GetComponentsInChildren<UnityEngine.UI.Text>();

		//get reference to the parent's canvas element
		UnityEngine.Canvas canvas = GetComponentInParent<UnityEngine.Canvas>();

		//get reference to canvas' components
		canvasGroup = canvas.GetComponent<UnityEngine.CanvasGroup>();
		canvasTransform = canvas.GetComponent<UnityEngine.RectTransform>();

		//get reference to timer panel's script
		GameObject countdownObj = GameObject.Find("CountdownTimer");
		countdownScript = (CountdownTimerScript) countdownObj.GetComponent(typeof(CountdownTimerScript));		
	}
	
	// Update is called once per frame
	void Update () {
		
	//	if (countdownScript.hasTimedOut())
	//		showQuestion();
	}

	public void OnMouseDown() {

		//display the question if on the answer screen
		if (!isShowingQuestion)
			showQuestion();

		else {
		//hide the panel if on the question screen
			//canvasGroup.alpha = 0.0f;
			//canvasGroup.interactable = false;
			canvasTransform.localScale = Vector3.zero;
		}

		isShowingQuestion = !isShowingQuestion;
	}

	public void showQuestion() {

		countdownScript.stopTimer();

		//show the question here
		foreach (UnityEngine.UI.Text txt in answerPanelText)
			txt.text = question;

	}

	public void showAnswer() {
		//show the question here
		foreach (UnityEngine.UI.Text txt in answerPanelText)
			txt.text = answer;
		
		countdownScript.startTimer();
	}
}
