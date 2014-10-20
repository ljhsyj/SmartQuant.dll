// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public class GroupDispatcher
    {
        private Framework framework;

        public GroupDispatcher(Framework framework)
        {
            this.framework = framework;
            framework.EventManager.Dispatcher.NewGroup += new GroupEventHandler((sender, args) =>
                {
                });
            framework.EventManager.Dispatcher.NewGroupEvent += new GroupEventEventHandler((sender, args) =>
                {
                });
            framework.EventManager.Dispatcher.NewGroupUpdate += new GroupUpdateEventHandler((sender, args) =>
                {
                });
            framework.EventManager.Dispatcher.FrameworkCleared += new FrameworkEventHandler((sender, args) =>
                {
                });
        }

        public void AddListener(IGroupListener listener)
        {
        }

        public void RemoveListener(IGroupListener listener)
        {
        }
    }
}
