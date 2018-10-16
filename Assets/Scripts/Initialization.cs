using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class ClueData {

	public string category;
	public int categoryId;
	public string[] question;
	public string[] answer;

	public ClueData() {

		category = "UNDEFINED";
		categoryId = 0;
		question = new string[5];
		answer = new string[5];
	}
}
class UjDatasetReader {

	IDbConnection dbConnection;
	IDbCommand dbCommand;
	public IDataReader dbReader;

	public UjDatasetReader (string db_path, string sqlQuery) {

		openConnection ("URI=file:" + db_path);

		queryDb (sqlQuery);
	}

	~UjDatasetReader () {

		dbReader.Close();
		dbCommand.Dispose();
		dbConnection.Close();

		dbReader = null;
		dbCommand = null;
		dbConnection = null;
	}

	private void openConnection (string path) {
		dbConnection = (IDbConnection) new SqliteConnection (path);
		dbConnection.Open();
		dbCommand = dbConnection.CreateCommand();
	}

	private void queryDb (string sql) {

		dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = sql;
		dbReader = dbCommand.ExecuteReader();
	}
}
public class Initialization : MonoBehaviour {

	[SerializeField]
	private string tableName;

	[SerializeField]
	private string categoryTableName;

	[SerializeField]
	int startValue = 100;

	[SerializeField]
	int incrementValue = 100;

	[SerializeField]
	int categoryCount = 6;

	[SerializeField]
	int clueCount = 6;

	private Dictionary<int,ClueData> clues;

	// Use this for initialization
	void Start () {

		setupCluePanels();
		loadDataset();
		setCategories();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape"))
			Application.Quit();
	}

	private void setupCluePanels() {

		string cluePanelToken = "CluePanel";

		//update the text on the clue panels
		for (int i = 1; i < categoryCount + 1; i++) {
			
			string panelCol = i.ToString();

			for (int j = 1; j < clueCount; j++) {

				string cluePanelName = cluePanelToken + panelCol + j.ToString();					
				
				GameObject cluePanel = GameObject.Find (cluePanelName);
					
				if (cluePanel == null)
					break;

				int clueValue = startValue + incrementValue * (j-1);

				UnityEngine.UI.Text[] textComponents = cluePanel.GetComponentsInChildren<UnityEngine.UI.Text>();

				foreach (UnityEngine.UI.Text txt in textComponents)
					txt.text = "$" + clueValue.ToString();

			}
		}		
	}

	private void setCategories() {

		int i = 1;

		foreach (ClueData c in clues.Values) {
			UnityEngine.UI.Text panelText = GameObject.Find("CategoryPanel" + i.ToString()).GetComponentInChildren<UnityEngine.UI.Text>();
			panelText.text = c.category.ToUpper();
			i++;
		}		
	}

	private void loadDataset() {
				
		string sql = "SELECT Difficulty, CategoryID, Round, Category, Question, Answer FROM " + tableName;
		sql += " INNER JOIN (SELECT DISTINCT ID as cat_id,CATEGORY FROM " + categoryTableName + "_category ORDER BY RANDOM() LIMIT 6) AS cat";
		sql += " ON " + tableName + ".CategoryID = cat.cat_id;";

		//selecct 6 random categories
		UjDatasetReader categories = 
			new UjDatasetReader("/home/joel/Projects/UnityJeopardy/Assets/Database/jeopardy_csv.db3", sql);

		IDataReader reader = categories.dbReader;

		clues = new Dictionary<int, ClueData>();

		while (reader.Read()) {
			int catId = reader.GetInt32(1);
			int difficulty = reader.GetInt32(0);
			string question = reader.GetString(4);
			string answer = reader.GetString(5);
			string category = reader.GetString(3);

			if (!clues.ContainsKey(catId))
				clues.Add(catId, new ClueData());

			clues[catId].categoryId = catId;
			clues[catId].category = category;
			clues[catId].question[difficulty-1] = question.ToUpper();
			clues[catId].answer[difficulty-1] = answer.ToUpper();
			print (catId);
			print (category);
			print (question);
			print (answer);
		}

		categories = null;
	}

	public Dictionary<int, ClueData> getClueData() {

		return clues;

	}
}
