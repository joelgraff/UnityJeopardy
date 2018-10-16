using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueOnClick : MonoBehaviour {

	[SerializeField]
	private GameObject clueDictionary;

	private RectTransform answerPanelTransform;

	private CanvasGroup answerPanelGroup;

	private UnityEngine.UI.Text[] clueButtonText;

	private float panelSpeed = 1.5f;

	private bool isExpanding = false;

	private float elapsedTime = 0.0f;

	private ClueData clue;

	private int clueDifficulty = 0;

	ClueDisplay answerPanelScript;

	public void OnClick() {

		if (clueButtonText[0].text == "")
			return;

		//set the answer panel pivot point to the clicked button
		//to prepare for the animation
		setAnswerPanelPivot();

		//update the answer panel with the question / answer
		answerPanelScript.question = clue.question[clueDifficulty-1];
		answerPanelScript.answer = clue.answer[clueDifficulty-1];
		answerPanelScript.showAnswer();

		//display the answer panel
		answerPanelGroup.alpha = 1.0f;
		answerPanelGroup.interactable = true;

		//clear the clue button text
		foreach (UnityEngine.UI.Text txt in clueButtonText)
			txt.text = "";

		isExpanding = true;
		elapsedTime = 0.0f;
	}

	// Use this for initialization
	void Start () {

		//reference to the answer panel elements for animation
		GameObject parent = GameObject.Find("AnswerPanelCanvas");

		answerPanelTransform = parent.GetComponent<RectTransform>();
		answerPanelGroup = parent.GetComponentInChildren<UnityEngine.CanvasGroup>();

		//reference to the text elements for later clearing
		clueButtonText = GetComponentsInChildren<UnityEngine.UI.Text>();

		//set difficulty and category values
		//char[] nameArr = name.ToCharArray();

		int clueCategory = int.Parse(name[9].ToString());
		clueDifficulty = int.Parse(name[10].ToString());

		//reference to the clue dictionary
		GameObject initObj = GameObject.Find("Initialization");
		Initialization initScript = (Initialization) initObj.GetComponent(typeof(Initialization));

		int i = 1;

		foreach (ClueData c in initScript.getClueData().Values) {
			if (i == clueCategory) {
				clue = c;
				break;
			}
			i++;
		}

		GameObject answerPanelObj = GameObject.Find("AnswerPanel");
		answerPanelScript = (ClueDisplay) answerPanelObj.GetComponent(typeof(ClueDisplay));
	}
	
	// Update is called once per frame
	void Update () {

		//animation loop lasts for two seconds on click
		//does smooth LERP from zero scale to full screen
		if (isExpanding) {

			Vector3 curScale = answerPanelTransform.localScale;

			if (curScale == Vector3.zero)
				curScale = new Vector3 (0.01f, 0.01f, 0.01f);

			answerPanelTransform.localScale = Vector3.Lerp (curScale, Vector3.one * 1.0f, panelSpeed * 4.0f * Time.deltaTime);

			elapsedTime += Time.deltaTime;

			isExpanding = (elapsedTime <= 2.0f);
		}
	}

	void setAnswerPanelPivot() {

		//calculates the pivot point as the ration of the actual position to the screen dimensions
		Vector2 resolution = GetComponentInParent<UnityEngine.UI.CanvasScaler>().referenceResolution;
		Vector2 absPos = (resolution / 2.0f) + new Vector2 (transform.localPosition.x, transform.localPosition.y);

		Vector2 pivotPoint = new Vector2 (absPos.x / resolution.x, absPos.y / resolution.y);

		answerPanelTransform.pivot = pivotPoint;
	}
}
