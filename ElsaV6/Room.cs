using System;
using System.Collections.Generic;
using System.Text;

namespace ElsaV6
{
    class Room
    {
        private string _roomName;

        public Room(string roomName)
        {
            _roomName = roomName;

            RoomID = Utils.Text.ToLowerAlphaNum(roomName);
        }

        public string RoomID { get; }
    }
}
