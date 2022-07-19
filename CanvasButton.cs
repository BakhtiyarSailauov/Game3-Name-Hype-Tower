using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CanvasButton : MonoBehaviour
{
  public Sprite musicOn, musicOff;

    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Music")
        { GetComponent<Image>().sprite = musicOff; }
    }
           
    public void restartGame() 
    {
        if (PlayerPrefs.GetString("music") != "No") { 
            GetComponent<AudioSource>().Play(); }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void openInstagram() 
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        Application.OpenURL("https://www.instagram.com/bakhtiyar__sailauov/");
    }

    public void loadShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
        { GetComponent<AudioSource>().Play();}

        SceneManager.LoadScene("Shop");
    }
    public void exitShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("Main");
    }
    public void musicWork() 
    {
        if (PlayerPrefs.GetString("music") == "No"){
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        else {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
