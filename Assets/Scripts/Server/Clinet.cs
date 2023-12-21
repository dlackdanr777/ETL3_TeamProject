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


    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Start()
    {
        tcp = new TCP();
    }

    //¼­¹ö ¿¬°á
    public void ConnectToServer()
    {
        tcp = new TCP();
        InitializeClientData();
        tcp.Connect();

    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receiveData;
        private byte[] receiveBuffer;

        public void Connect()
        {
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

            //¼ÒÄÏÀÌ ¿¬°áµÇÀÖ´Ù¸é 
            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receiveData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if(_byteLength <= 0)
                {
                    //¿¬°á ²÷±è
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receiveData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                //¿¬°á ²÷±è
            }

        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receiveData.SetBytes(_data);

            if(receiveData.UnreadLength() >= 4)
            {
                _packetLength = receiveData.ReadInt();
                if(_packetLength <= 0)
                {
                    return true;
                }
            }

            while(_packetLength > 0 && _packetLength <= receiveData.UnreadLength())
            {
                byte[] _packetBytes = receiveData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using(Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receiveData.UnreadLength() >= 4)
                {
                    _packetLength = receiveData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if(_packetLength <= 1)
            {
                return true;
            }

            return false;
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome }
        };
        Debug.Log("Initialized packet...");
    }
}

