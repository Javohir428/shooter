using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour {

	public Button restartButton;
	
    void Start()
    {
        Button btn = restartButton.GetComponent<Button>();
        //Calls the TaskOnClick method when you click the Button
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Output this to console when the Button is clicked
        SceneManager.LoadScene ("proto");
    }
}
