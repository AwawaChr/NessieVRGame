using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore; // Necesario para cargar escenas

public class MainMenu : MonoBehaviour
{
    [Header("Paneles (Opcional si usas escenas para todo)")]
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelConfiguracion;

    private void Start()
    {
        panelMenu = GameObject.Find("MenuPrincipal");
        panelMenu.SetActive(true);
        panelConfiguracion = GameObject.Find("Configuracion");
        panelConfiguracion.SetActive(false);
    }
    // M�todos para cargar escenas independientes
    public void VerVideo()
    {
        SceneManager.LoadScene("VideoScene");
    }

    public void AbrirConfiguracion()
    {
        panelMenu.SetActive(false);
        panelConfiguracion.SetActive(true);
    }

    public void VolverMenu()
    {
        panelConfiguracion.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}