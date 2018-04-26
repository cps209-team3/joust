//-----------------------------------------------------------
//  File:   ISerializable.cs
//  Desc:   Holds the ISerializable Interface definition
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public interface ISerializable
    {
        // Converts properties of object to string
        string Serialize();
        // Parses a string of properties and adds them to the object which called it
        void Deserialize(string data);
    }
}
