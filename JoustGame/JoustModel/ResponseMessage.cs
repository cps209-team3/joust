//-----------------------------------------------------------
//File:   ResponseMessage.cs
//Desc:   Defines the response classes to be sent from the server
//        to the client.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;

namespace JoustModel
{
    public class ResponseMessage
    {
      
    }

    public class PlayerMoveResponseMessage: ResponseMessage
    {
        public int Index { get; set; }
        public double Speed { get; set; }
        public double Angle { get; set; }
    }

    public class GameStatusResponseMessage: ResponseMessage
    {
        public int Index { get; set; }
        public double Speed { get; set; }
        public double Angle { get; set; }
    }
}