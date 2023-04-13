using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class UDPClient : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint remoteEP;

    async void Start()
    {
        // Create a UDP socket and connect to the server
        udpClient = new UdpClient();
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

        // Use Task to receive in a separate thread
        await Task.Run(async () =>
        {
            while (true)
            {
                // transmission process
                var sendData = System.Text.Encoding.UTF8.GetBytes("Hello, server!");
                await udpClient.SendAsync(sendData, sendData.Length, remoteEP);

                // reception process
                var receivedResult = await udpClient.ReceiveAsync();

                // Process received data
                var receivedData = receivedResult.Buffer;
                var receivedDataStr = System.Text.Encoding.UTF8.GetString(receivedData);
                Debug.Log($"Received: {receivedDataStr} from {receivedResult.RemoteEndPoint}");
            }
        });
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
