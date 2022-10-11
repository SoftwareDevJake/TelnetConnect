using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// telent
using System.Net;
using System.Net.Sockets;
using System.Text;
// telent

public class TelnetConnect : MonoBehaviour
{
    public Socket client;

    public Text serverMessage;

    public void ConnectTelnet(string ip, int port)
    {
        // telent
        byte[] data = new byte[2048];

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);

        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            client.Connect(ipep);
            Debug.Log("connect to server.");
            serverMessage.text = "연결 성공";
        }
        catch (SocketException e)
        {
            Debug.Log("not connect to server.");
            Debug.Log(e.ToString());
            serverMessage.text = "연결 실패 (서버가 켜져있는지 확인해주세요)";
            return;
        }

        // telent
    }

    public void LoginIntoTelnet(string id, string pw)
    {
        byte[] bytes = new byte[2048];

        try
        {
            byte[] msg = Encoding.ASCII.GetBytes("Login \"" + id + "\" \"" + pw + "\"\r\n");

            // Send the data through the socket.
            int bytesSent = client.Send(msg);

            // Receive the response from the remote device.
            int bytesRec = client.Receive(bytes);
            Debug.Log("Echoed test = " + Encoding.ASCII.GetString(bytes, 0, bytesRec));

        }
        catch (SocketException se)
        {
            Debug.Log("SocketException : " + se.ToString());
        }
    }

    public void SendCommand(string command)
    {
        byte[] msg = Encoding.UTF8.GetBytes("" + command + Strings.ENTER);
        byte[] bytes = new byte[1024];

        int byteCount = client.Send(msg, 0, msg.Length, SocketFlags.None);
        //Debug.Log("Sent {0} bytes.  " + byteCount);

        //// Get reply from the server.
        byteCount = client.Receive(bytes, 0, bytes.Length, SocketFlags.None);

        //if (byteCount > 0)
        //    Debug.Log(Encoding.UTF8.GetString(bytes, 0, byteCount));
    }
}
