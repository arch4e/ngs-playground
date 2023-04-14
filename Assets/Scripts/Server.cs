using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UDPServer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI logs;

    private UdpClient udpServer;
    private IPEndPoint remoteEP;

    private int port = 7777;
    private List<Tuple<string, string>> matchList = new List<Tuple<string, string>>();
    private List<string> waitList = new List<string>();

    async void Start()
    {
        // Create a UDP socket and listen on the specified port
        udpServer = new UdpClient(port);

        // Use Task to receive in a separate thread
        await Task.Run(async () =>
        {
            while (true)
            {
                // reception process
                var receivedResult = await udpServer.ReceiveAsync();

                // Process received data
                var receivedData = receivedResult.Buffer;
                var receivedDataStr = System.Text.Encoding.UTF8.GetString(receivedData);

                if (waitList.Exists(client => client == receivedResult.RemoteEndPoint.ToString()) ||
                    matchList.Exists(match => match.Item1 == receivedResult.RemoteEndPoint.ToString() ||
                                              match.Item2 == receivedResult.RemoteEndPoint.ToString())) {
                    // ToDo: reply function
                } else {
                    waitList.AddRange(new string[] { receivedResult.RemoteEndPoint.ToString() });
                    Debug.Log("Info: connect new player");
                    waitList.ForEach(client => Debug.Log(client));

                    if (waitList.Count >= 2) {
                        matchList.Add(Tuple.Create(waitList[0].ToString(), waitList[1].ToString()));
                        Debug.Log($"match list update:");
                        matchList.ForEach(match => Debug.Log(match));
                    }
                }

                // send process
                var sendData = System.Text.Encoding.UTF8.GetBytes("Hello, client!");
                await udpServer.SendAsync(sendData, sendData.Length, receivedResult.RemoteEndPoint);
            }
        });
    }

    void OnDestroy()
    {
        udpServer.Close();
    }
}
