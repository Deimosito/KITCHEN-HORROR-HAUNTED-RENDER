using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonEscena : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToPrincipalScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
