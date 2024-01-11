using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    // Start is called before the first frame update
    public string roomId;
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void CreateLobby()
    {
        CreateLobbyOptions options = new CreateLobbyOptions();
        string lobbyName = "My lobby";
        int maxPlayers = 2;
        options.IsLocked = true;
        Unity.Services.Lobbies.Models.Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers , options);
        Debug.Log("Lobby Created with Id " + lobby.LobbyCode);
        roomId = lobby.LobbyCode;
    }
    
}
