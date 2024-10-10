using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLvl1 : MonoBehaviour
{
    public void BackToLevel1()
    {
        SceneManager.LoadScene("LVL1");
    }

}
