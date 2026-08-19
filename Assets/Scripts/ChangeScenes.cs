using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
   public void GoToPrincipalScene()
    {
        SceneManager.LoadScene("Sample Scene");
    }
}
