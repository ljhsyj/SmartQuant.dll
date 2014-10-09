// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SmartQuant
{
    public class Group : DataObject
    {
        public override byte TypeId
        {
            get
            {
                return DataObjectType.Group;
            }
        }

        public int Id { get; internal set; }

        public string Name { get; private set; }

        public Framework Framework { get; internal set; }

        public Dictionary<string, GroupField> Fields { get; private set; }

        public List<GroupEvent> Events { get; private set; }

        public Group(string name)
        {
            Name = name;
            Fields = new Dictionary<string, GroupField>();
            Events = new List<GroupEvent>();
        }

        public void Add(string name, byte type, object value)
        {
            Add(new GroupField(name, type, value));
        }

        public void Add(string name, Color color)
        {
            Add(new GroupField(name, DataObjectType.Color, color));
        }

        public void Add(string name, string value)
        {
            Add(new GroupField(name, DataObjectType.String, value));
        }

        public void Add(string name, int value)
        {
            Add(new GroupField(name, DataObjectType.Int, value));
        }

        public void Add(string name, bool value)
        {
            Add(new GroupField(name, DataObjectType.Boolean, value));
        }

        public void Add(string name, DateTime dateTime)
        {
            Add(new GroupField(name, DataObjectType.DateTime, dateTime));
        }
       
        public void Remove(string fieldName)
        {
            throw new NotImplementedException();
        }

        public void OnNewGroupEvent(GroupEvent groupEvent)
        {
            throw new NotImplementedException();
        }

        private void Add(GroupField groupField)
        {
            throw new NotImplementedException();
        }
    }
}
