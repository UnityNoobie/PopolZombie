using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	public string levelName;

	void OnClick ()
	{
		if (!string.IsNullOrEmpty(levelName))
		{
			//Application.LoadLevel(levelName);
			SceneManager.LoadScene(levelName);
		}
	}
}