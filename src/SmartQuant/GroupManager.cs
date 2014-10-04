// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Collections.Generic;

namespace SmartQuant
{
    public class GroupManager
    {
        public IdArray<Group> Groups { get; private set; }

        public List<Group> GroupList  { get; private set; }

        public GroupManager(Framework framework)
        {
            this.Groups = new IdArray<Group>();
            this.GroupList = new List<Group>();
        }

        public void Add(Group group)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            this.Groups.Clear();
            this.GroupList.Clear();
        }
    }
}
