
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sonidos de interfaz")]
    public AudioClip sonidoBoton;
    public AudioClip sonidoCambioPantalla;
    public AudioClip sonidoInicioPartida;
    public AudioClip sonidoPerder;
    public AudioClip sonidoGanar;

    [Header("Música de fondo")]
    public AudioClip musicaFondo;

    private AudioSource efectos;
    private AudioSource musica;

    [Header("Volumen efectos")]
    public float volumenEfectos = 1f;
    private Slider sliderEfectos;

    [Header("Volumen música")]
    public float volumenMusica = 1f;
    private Slider sliderMusica;

    void Awake()
    {
        // Evitar duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Crear fuentes de audio
        efectos = gameObject.AddComponent<AudioSource>();
        musica = gameObject.AddComponent<AudioSource>();

        efectos.playOnAwake = false;

        musica.playOnAwake = false;
        musica.loop = true;
        musica.volume = volumenMusica;

        if (musicaFondo != null)
        {
            musica.clip = musicaFondo;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    // Controlar música al cambiar de escena
    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == "MenuPrincipal")
        {
            ReproducirMusica();
        }
        else
        {
            DetenerMusica();
        }
    }

    // Configurar sliders de la escena actual
    public void ConfigurarSliders(Slider sliderEfectosUI, Slider sliderMusicaUI)
    {
        sliderEfectos = sliderEfectosUI;
        sliderMusica = sliderMusicaUI;

        if (sliderEfectos != null)
        {
            sliderEfectos.onValueChanged.RemoveListener(CambiarVolumenEfectos);
            sliderEfectos.SetValueWithoutNotify(volumenEfectos);
            sliderEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
        }

        if (sliderMusica != null)
        {
            sliderMusica.onValueChanged.RemoveListener(CambiarVolumenMusica);
            sliderMusica.SetValueWithoutNotify(volumenMusica);
            sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        }
    }

    public void CambiarVolumenEfectos(float volumen)
    {
        volumenEfectos = volumen;
    }

    public void CambiarVolumenMusica(float volumen)
    {
        volumenMusica = volumen;
        musica.volume = volumenMusica;
    }

    // Sonidos de interfaz
    public void SonidoBoton()
    {
        if (sonidoBoton != null)
            efectos.PlayOneShot(sonidoBoton, volumenEfectos);
    }

    public void SonidoCambioPantalla()
    {
        if (sonidoCambioPantalla != null)
            efectos.PlayOneShot(sonidoCambioPantalla, volumenEfectos);
    }

    public void SonidoInicioPartida()
    {
        if (sonidoInicioPartida != null)
            efectos.PlayOneShot(sonidoInicioPartida, volumenEfectos);
    }

    public void SonidoPerdiste()
    {
        if (sonidoPerder != null)
            efectos.PlayOneShot(sonidoPerder, volumenEfectos);
    }

    public void SonidoGanaste()
    {
        if (sonidoGanar != null)
            efectos.PlayOneShot(sonidoGanar, volumenEfectos);
    }

    // Reproducir música
    public void ReproducirMusica()
    {
        if (musicaFondo != null && !musica.isPlaying)
        {
            musica.clip = musicaFondo;
            musica.Play();
        }
    }

    // Detener música
    public void DetenerMusica()
    {
        musica.Stop();
    }
}
