using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using System.Text;

public class Client : MonoBehaviour
{
    bool socketReady;
    TcpClient socket;
    NetworkStream stream;
    StreamWriter writer;
    StreamReader reader;
    //private const string SERVER_IP = "127.0.0.1";
    private const string SERVER_IP = "34.64.70.123";

    private const int SERVER_PORT = 7777; // 클라이언트 포트 번호 설정

    public void ConnectToServer()
    {
        if (socketReady) return;

        try
        {
            socket = new TcpClient(SERVER_IP, SERVER_PORT);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
            Debug.Log("서버에 연결되었습니다.");
            SendClientName("Guest" + UnityEngine.Random.Range(1000, 10000));
        }
        catch (Exception e)
        {
            Debug.LogError($"연결 실패: {e.Message}");
            socketReady = false;
        }

    }

    void Start()
    {
        // 서버가 시작된 후에 클라이언트를 연결하도록 딜레이를 줄 수 있습니다.
        StartCoroutine(ConnectAfterDelay());
    }

    private IEnumerator ConnectAfterDelay()
    {
        yield return new WaitForSeconds(3); // 서버가 시작될 시간을 주기 위해 3초 대기
        ConnectToServer();
    }

    void Update()
    {
        if (socketReady && stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
                Debug.Log(data); // 수신된 데이터 출력
        }
    }

    void SendClientName(string clientName)
    {
        Send($"&NAME|{clientName}");
    }

    void Send(string data)
    {
        if (!socketReady) return;

        writer.WriteLine(data);
        writer.Flush();
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void CloseSocket()
    {
        if (!socketReady) return;

        try
        {
            writer.Close();
            reader.Close();
            socket.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            socketReady = false;
        }
    }
}
