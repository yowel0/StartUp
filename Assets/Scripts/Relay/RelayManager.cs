using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using System;

public class RelayManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI joinCodeText;
    [SerializeField]
    private TMP_InputField joinCodeInputField;

    bool joined = false;

    private async void Start(){
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void StartRelay(){
        if (joined == false){
            try{
                string joinCode = await StartHostWithRelay();
                joinCodeText.text = joinCode;
            }
            catch{
                print("failed to start relay");
            }
            joined = true;
        }
    }

    public async void JoinRelay(){
        if (joined == false){
            try{
                await StartClientWithRelay(joinCodeInputField.text);
            }
            catch{
                joinCodeText.text = "invalid Code";
                return;
            }
            joinCodeText.text = joinCodeInputField.text;
            joined = true;
        }
    }

    private async Task<string> StartHostWithRelay(int maxConnections = 3){
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    private async Task<bool> StartClientWithRelay(string joinCode){
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}
