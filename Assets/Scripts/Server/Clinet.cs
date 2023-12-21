using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Clinet : SingletonHandler<Clinet>
{
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 50001;
    public int myId = 0;
    public TCP tcp;


    private void Start()
    {
        tcp = new TCP();
    }

    //서버 연결
    public void ConnectToServer()
    {
        tcp = new TCP();
        tcp.Connect();
        
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect()
        {
            Debug.Log("Connect!");
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(Instance.ip, Instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            //소켓이 연결되있다면 
            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {

            }
            catch
            {

            }

        }
    }
}

