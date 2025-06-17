using UnityEngine;
using UnityEngine.SceneManagement;

public class GORespawnScript : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   
}
