using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;
        public Vector2 position;
        public Quaternion rotation;

        private float moveSpeed = 1f / Constants.TICKS_PER_SEC;
        private Vector2 inputs;

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;
            inputs = new Vector2(0, 0);
        }

        public void Update()
        {
            Vector2 _inputDirection = new Vector2(0, 0);
            _inputDirection.Y = inputs.Y;
            _inputDirection.X = inputs.X;
            Console.WriteLine($"_inputDirection.X: {_inputDirection.X}, _inputDirection.Y: {_inputDirection.Y}");

            Move(_inputDirection);
        }

        private void Move(Vector2 _inputDirection)
        {
            position += _inputDirection * moveSpeed;
            Console.WriteLine($"position : {position}");
            
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
        public void SetInput(Vector2 _inputs, Quaternion _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}