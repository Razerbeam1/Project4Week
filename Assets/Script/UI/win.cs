using UnityEngine;
using UnityEngine.SceneManagement;

public class win : MonoBehaviour
{

    public GameObject Main_menu;
    public GameObject credit;
    public GameObject WIN;

    public void Mainmenu ()
    {
        WIN.SetActive(false);
        Main_menu.SetActive(true);
    }

    public void Credit()
    {
        WIN.SetActive(false);
        credit.SetActive(true);
    }



}