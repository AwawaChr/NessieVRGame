using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class MenuPausa : MonoBehaviour
{
    [Header("Input Quest 3 (Botón 3 barras - Mando Izquierdo)")]
    [SerializeField] private InputActionProperty botonPausaAction;

    [Header("Canvas y Paneles")]
    [SerializeField] private GameObject canvasPausa;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelConfiguracion;

    [Header("Posicionamiento frente a la vista")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float distanciaFrenteJugador = 1.6f;
    [SerializeField] private float offsetVertical = -0.1f;

    [Header("Audio (Conectado a AudioManager)")]
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderEfectos;

    [Header("Confort VR (Viñeta y Altura)")]
    [SerializeField] private TunnelingVignetteController vignetteController;
    [SerializeField] private Toggle toggleVineta;
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private Slider sliderAltura;

    [Header("Locomoción (Dejar en None si bloquea el clic en pausa)")]
    [SerializeField] private GameObject locomotionSystem;

    private bool estaPausado = false;
    private float alturaBaseCamera;
    private float fixedDeltaTimeOriginal;

    private void Start()
    {
        fixedDeltaTimeOriginal = Time.fixedDeltaTime;

        if (xrOrigin != null)
            alturaBaseCamera = xrOrigin.CameraYOffset;

        CargarPreferenciasConfort();
        canvasPausa.SetActive(false);
    }

    private void OnEnable()
    {
        if (botonPausaAction.action != null)
        {
            botonPausaAction.action.Enable();
            botonPausaAction.action.performed += AlPulsarBotonPausa;
        }
    }

    private void OnDisable()
    {
        if (botonPausaAction.action != null)
            botonPausaAction.action.performed -= AlPulsarBotonPausa;
    }

    private void AlPulsarBotonPausa(InputAction.CallbackContext context)
    {
        if (estaPausado)
            Resumir();
        else
            Pausar();
    }

    public void Pausar()
    {
        estaPausado = true;
        Time.timeScale = 0.0001f;
        Time.fixedDeltaTime = fixedDeltaTimeOriginal * Time.timeScale;

        if (locomotionSystem != null)
            locomotionSystem.SetActive(false);

        PosicionarFrenteAJugador();

        canvasPausa.SetActive(true);
        AbrirPanelPausa();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConfigurarSliders(sliderEfectos, sliderMusica);
            AudioManager.Instance.SonidoCambioPantalla();
        }
    }

    public void Resumir()
    {
        estaPausado = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = fixedDeltaTimeOriginal;

        if (locomotionSystem != null)
            locomotionSystem.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.SonidoBoton();

        canvasPausa.SetActive(false);
    }

    public void AbrirConfiguracion()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConfigurarSliders(sliderEfectos, sliderMusica);
            AudioManager.Instance.SonidoCambioPantalla();
        }

        panelPausa.SetActive(false);
        panelConfiguracion.SetActive(true);
    }

    public void AbrirPanelPausa()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SonidoCambioPantalla();

        panelConfiguracion.SetActive(false);
        panelPausa.SetActive(true);
    }

    public void VolverAlMenuPrincipal()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SonidoBoton();

        Time.timeScale = 1f;
        Time.fixedDeltaTime = fixedDeltaTimeOriginal;
        SceneManager.LoadScene("SampleScene");
    }

    private void PosicionarFrenteAJugador()
    {
        if (mainCamera == null || canvasPausa == null) return;

        Vector3 direccionHorizontal = new Vector3(mainCamera.forward.x, 0f, mainCamera.forward.z).normalized;
        if (direccionHorizontal.sqrMagnitude < 0.001f)
            direccionHorizontal = mainCamera.forward;

        Vector3 posicionObjetivo = mainCamera.position + direccionHorizontal * distanciaFrenteJugador;
        posicionObjetivo.y = mainCamera.position.y + offsetVertical;

        canvasPausa.transform.position = posicionObjetivo;
        canvasPausa.transform.rotation = Quaternion.LookRotation(direccionHorizontal);
    }

    // --- OPCIONES DE CONFORT VR ---

    public void CambiarVineta(bool activa)
    {
        if (vignetteController != null)
            vignetteController.gameObject.SetActive(activa);
        PlayerPrefs.SetInt("VinetaActiva", activa ? 1 : 0);
    }

    public void CambiarAltura(float offset)
    {
        if (xrOrigin != null)
            xrOrigin.CameraYOffset = alturaBaseCamera + offset;
        PlayerPrefs.SetFloat("AlturaOffset", offset);
    }

    private void CargarPreferenciasConfort()
    {
        bool vineta = PlayerPrefs.GetInt("VinetaActiva", 1) == 1;
        float altura = PlayerPrefs.GetFloat("AlturaOffset", 0f);

        if (toggleVineta != null) toggleVineta.isOn = vineta;
        if (sliderAltura != null) sliderAltura.value = altura;

        CambiarVineta(vineta);
        CambiarAltura(altura);
    }
}