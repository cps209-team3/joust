﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public interface ISerializable
    {
        string Serialize();
        void Deserialize();
    }
}
