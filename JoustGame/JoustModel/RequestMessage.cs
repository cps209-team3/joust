//-----------------------------------------------------------
//File:   RequestMessage.cs
//Desc:   Defines the message classes to be sent from the client
//        to the server.
//----------------------------------------------------------- 

using System;

namespace JoustModel
{
    public abstract class RequestMessage
    {
        // Executes in server program to process the request
        public abstract ResponseMessage Execute(GameController ctrl);
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
        public override ResponseMessage Execute(GameController ctrl)
        {
            GameStatusResponseMessage response = new GameStatusResponseMessage();

            return response;
        }
    }
}