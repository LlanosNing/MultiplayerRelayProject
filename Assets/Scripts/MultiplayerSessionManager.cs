using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSessionManager : MonoBehaviour
{
    [SerializeField] private int maxPlayers = 20;

    public ISession CurrentSession { get; private set; }

    private Task initializationTask;

    [Header("UI")]
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private TMP_Text joinCodeTxt;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        initializationTask = InitializeServices();
    }

    private async Task InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();

            Debug.Log($"UGS inicializado. Estado: {UnityServices.State}");

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            Debug.Log( $"Autenticado. Player ID: " + $"{AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogError("Error inicializando Unity Services:");
            Debug.LogException(e);

            throw;
        }
    }


    public async Task<string> CreateGame(int maxPlayers)
    {
        // Esperamos SIEMPRE a que UGS esté preparado
        await initializationTask;

        var options = new SessionOptions
        {
            MaxPlayers = maxPlayers
        }
        .WithRelayNetwork();

        CurrentSession =
            await MultiplayerService.Instance
                .CreateSessionAsync(options);

        Debug.Log($"Partida creada. Código: {CurrentSession.Code}");

        return CurrentSession.Code;
    }

    public async Task JoinGame(string joinCode)
    {
        await initializationTask;

        CurrentSession =await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);

        Debug.Log("Conectado a la partida.");
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsHost == false) return;

        NetworkManager.Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    #region Código para los botones

    public async void CreateGameButton()
    {
        try
        {
            string code = await CreateGame(maxPlayers);

            joinCodeTxt.text = $"CÓDIGO: {code}";
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void JoinGameButton()
    {
        try
        {
            string code = joinCodeInput.text.Trim().ToUpper();

            await JoinGame(code);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    #endregion
}