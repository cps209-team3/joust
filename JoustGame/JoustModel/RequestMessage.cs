//-----------------------------------------------------------
//File:   RequestMessage.cs
//Desc:   Defines the message classes to be sent from the client
//        to the server.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;

namespace JoustModel
{
    public abstract class RequestMessage
    {
        // Executes in server program to process the request
        public abstract ResponseMessage Execute(GameController ctrl);
    }

    public class InitialRequestMessage : RequestMessage
    {
        public string PlayerName { get; set; }

        public InitialRequestMessage(string playerName)
        {
            PlayerName = playerName;
        }

        public override ResponseMessage Execute(GameController ctrl)
        {
            InitialResponseMessage response = new InitialResponseMessage();
            
            ctrl.WorldRef.playerNames.Add(PlayerName);
            response.PlayerNames = ctrl.WorldRef.playerNames;
            response.CanHost = !(ctrl.WorldRef.hosted);

            return response;
        }
    }

    public class StartRequestMessage : RequestMessage
    {
        public StartRequestMessage() { }

        public override ResponseMessage Execute(GameController ctrl)
        {
            StartResponseMessage response = new StartResponseMessage();

            ctrl.SetPlatforms();
            ctrl.InitializePlayers();
            ctrl.WorldRef.multiplayerStarted = true;

            return response;
        }
    }

    public class LobbyRequestMessage : RequestMessage
    {
        public LobbyRequestMessage() { }

        public override ResponseMessage Execute(GameController ctrl)
        {
            LobbyResponseMessage response = new LobbyResponseMessage();

            response.PlayerNames = ctrl.WorldRef.playerNames;
            response.Start = ctrl.WorldRef.multiplayerStarted;

            return response;
        }
    }

    public class PlayerMoveRequestMessage : RequestMessage
    {
        public int Index { get; set; }
        public double Speed { get; set; }
        public double Angle { get; set; }

        public PlayerMoveRequestMessage(int index, double speed, double angle)
        {
            Index = index;
            Speed = speed;
            Angle = angle;
        }

        public override ResponseMessage Execute(GameController ctrl)
        {
            PlayerMoveResponseMessage response = new PlayerMoveResponseMessage();

            response.Index = Index;
            response.Speed = Speed;
            response.Angle = Angle;
            //ctrl.MovePlayer(response.Index, response.Speed, response.Angle);
            return response;
        }
    }

    public class GameStatusRequestMessage : RequestMessage
    {
        public GameStatusRequestMessage() { }

        public override ResponseMessage Execute(GameController ctrl)
        {
            GameStatusResponseMessage response = new GameStatusResponseMessage();

            response.PlayerNames = ctrl.WorldRef.playerNames;
            response.Platforms = ctrl.WorldRef.platforms;

            return response;
        }
    }
}