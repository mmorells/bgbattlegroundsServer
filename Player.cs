using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;
        public Vector2 position;
        public Quaternion rotation;

        private float moveSpeed = 0.2f / Constants.TICKS_PER_SEC;
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
            List<Location> pathlist = new List<Location>();
            if (position.X == _inputDirection.X && position.Y == _inputDirection.Y) {
                pathlist.Clear();
                return;
            }
            //pathlist.Clear();
            var resp = Pathfinding.CalcPath(position, _inputDirection, "map");
            pathlist = resp.Item1;
            var offsetX = resp.Item2;
            var offsetY = resp.Item3;
            Console.WriteLine("yoyoyo:" + pathlist.Count);
            //pathlist = Pathfinding.CalcPath(position, _inputDirection, "map");
            while (pathlist.Count > 0)
            {
                Location path = pathlist.First();
                _inputDirection.X = path.X - offsetX;
                _inputDirection.Y = path.Y - offsetY;
                Console.WriteLine("pathX: " + (path.X - offsetX + " | positionX: " + position.X));
                Console.WriteLine("pathY: " + (path.Y - offsetY + " | positionY: " + position.Y));
                position += _inputDirection * moveSpeed;
                Console.WriteLine($"position : {position}");
                if (position.X == path.X && position.Y == path.Y)
                {
                    pathlist.Remove(path);
                }
                while (position.X != path.X && position.Y != path.Y)
                {
                    ServerSend.PlayerPosition(this);
                    ServerSend.PlayerRotation(this);
                    if (position.X == path.X && position.Y == path.Y)
                    {
                        pathlist.Remove(path);
                    }
                }
                /* _inputDirection.X = path.X - offsetX;
                _inputDirection.Y = path.Y - offsetY;
                position += _inputDirection * moveSpeed;
                Console.WriteLine($"position : {position}");
                ServerSend.PlayerPosition(this);
                ServerSend.PlayerRotation(this); */
                /* if (position == _inputDirection)
                {
                    position += _inputDirection * moveSpeed;
                    Console.WriteLine($"position : {position}");
                    ServerSend.PlayerPosition(this);
                    ServerSend.PlayerRotation(this);
                } */
            }
        }

        public void SetInput(Vector2 _inputs, Quaternion _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}