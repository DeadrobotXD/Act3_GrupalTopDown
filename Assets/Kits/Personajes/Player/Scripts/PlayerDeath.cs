using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [Header("UI de Game Over")]
    [Tooltip("Panel que se muestra al morir. Debe empezar DESACTIVADO en el editor.")]
    [SerializeField] GameObject gameOverPanel;

    [Tooltip("Si está activo, congela el juego (Time.timeScale = 0) al morir.")]
    [SerializeField] bool pauseOnDeath = true;

    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health != null) health.onLifeDepleted.AddListener(OnPlayerDied);
    }

    private void OnDisable()
    {
        if (health != null) health.onLifeDepleted.RemoveListener(OnPlayerDied);
    }

    private void Start()
    {
        // El panel empieza oculto.
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void OnPlayerDied(float startLife)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // Congelamos el juego para que los zombis dejen de moverse
        // mientras se muestra el Game Over.
        if (pauseOnDeath) Time.timeScale = 0f;
    }

    // Este método lo llama el botón "Reintentar" desde el Inspector (OnClick).
    public void RestartScene()
    {
        // Importante: restauramos el tiempo antes de recargar, porque
        // Time.timeScale se conserva entre escenas.
        Time.timeScale = 1f;

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    // Opcional: para un botón de "Salir" del juego.
    public void QuitGame()
    {
        Application.Quit();
    }
}