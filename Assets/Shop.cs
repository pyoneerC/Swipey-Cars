using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    public void GoToFirstLevel()
    {
        SceneManager.LoadScene("LVL1");
    }

    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoToInventory()
    {
        SceneManager.LoadScene("Inventory");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
