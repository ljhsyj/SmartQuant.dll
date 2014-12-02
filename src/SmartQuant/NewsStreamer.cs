// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.IO;

namespace SmartQuant
{
    public class NewsStreamer : ObjectStreamer
    {
        public NewsStreamer()
        {
            this.typeId = DataObjectType.News;
            this.type = typeof(News);
        }

        public override object Read(BinaryReader reader)
        {
            var version = reader.ReadByte();
            var news = new News();
            news.DateTime = new DateTime(reader.ReadInt64());
            news.ProviderId = reader.ReadInt32();
            news.InstrumentId = reader.ReadInt32();
            news.Urgency = reader.ReadByte();
            news.Url = reader.ReadString();
            news.Headline = reader.ReadString();
            news.Text = reader.ReadString();
            return news;
        }

        public override void Write(BinaryWriter writer, object obj)
        {
            var news = obj as News;
            byte version = 0;
            writer.Write(version);
            writer.Write(news.DateTime.Ticks);
            writer.Write(news.ProviderId);
            writer.Write(news.InstrumentId);
            writer.Write(news.Urgency);
            writer.Write(news.Url);
            writer.Write(news.Headline);
            writer.Write(news.Text);
        }
    }
}
