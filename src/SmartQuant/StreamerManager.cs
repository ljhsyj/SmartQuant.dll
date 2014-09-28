// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace SmartQuant
{
	public class StreamerManager
	{
        private Dictionary<Type, ObjectStreamer> streamers = new Dictionary<Type, ObjectStreamer>(); 

        public void Add(ObjectStreamer streamer)
        {     
            streamer.streamerManager = this;
            streamers.Add(streamer.GetType(), streamer);
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
        }

        public object Deserialize(BinaryReader reader)
        {
            return null;
        }
	}
}
