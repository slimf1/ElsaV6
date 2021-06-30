using System;
using System.Collections.Generic;
using System.Text;

namespace ElsaV6
{
    public class Room
    {
        private string _roomName;

        public Room(string roomName, string roomID = null)
        {
            _roomName = roomName;
            
            Users = new Dictionary<string, User>();
            RoomID = string.IsNullOrEmpty(roomID)
                ? Utils.Text.ToLowerAlphaNum(roomName)
                : roomID;
        }

        public IDictionary<string, User> Users { get; }

        public Room(string roomName)
        {
            _roomName = roomName;

            RoomID = Utils.Text.ToLowerAlphaNum(roomName);
        }

        public string RoomID { get; }

        public override int GetHashCode()
        {
            return RoomID.GetHashCode();
        }

        public void InitializeUsers(string[] users)
        {
            foreach(string user in users)
            {
                JoinUser(user);
            }
        }

        public void JoinUser(string username)
        {
            var user = new User(username);
            Users[user.UserID] = user;
        }

        public void LeaveUser(string username)
        {
            var userID = Utils.Text.ToLowerAlphaNum(username);
            UpdateUserOntime(userID);
            Users.Remove(userID);
        }

        public void RenameUser(string oldName, string newName)
        {
            LeaveUser(oldName);
            JoinUser(newName);
        }

        public void UpdateUserOntime(string userID)
        {

        }
    }
}
