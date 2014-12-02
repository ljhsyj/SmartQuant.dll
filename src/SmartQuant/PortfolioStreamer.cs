// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class PortfolioStreamer : ObjectStreamer
    {
        public PortfolioStreamer()
        {
            this.typeId = DataObjectType.Portfolio;
            this.type = typeof(Portfolio);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            return new Portfolio(null, reader.ReadString());
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            byte version = 0;
            var portfolio =  obj as Portfolio;
            writer.Write(version);
            writer.Write(portfolio.Name);
        }
    }
}
