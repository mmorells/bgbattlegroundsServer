using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {_msg}");
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            Vector2 _inputs = new Vector2(0, 0);
            /* for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadFloat();
            } */
            _inputs = _packet.ReadVector2(true);
            Console.WriteLine($"Inputs: {_inputs}");
            Quaternion _rotation = _packet.ReadQuaternion();
            //Console.WriteLine($"_inputs: {_inputs}, _rotation: {_rotation}");
            Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
        }
    }
}