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
                Debug.Log($"Received: {receivedDataStr} from {receivedResult.RemoteEndPoint}");

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

    void PrintLog(string text)
    {
        logs.text += $"\n{text}";
    }
}
