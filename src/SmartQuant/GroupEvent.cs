// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class GroupEvent : DataObject
	{
        public Group Group { get; set; }

        public Event Obj { get; private set; }

        public int GroupId { get; private set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.GroupEvent;
            }
        }

        public GroupEvent(Event obj, Group group)
        {
            Obj = obj;
            Group = group;
            GroupId = group != null ? group.Id : -1;
        }

        internal GroupEvent(Event obj, int groupId)
        {
            Obj = obj;
            GroupId = groupId;
        }
	}
}
