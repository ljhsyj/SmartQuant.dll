// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

namespace SmartQuant
{
    public interface IProvider
    {
        ProviderStatus Status { get; }

        bool IsConnected { get; }

        bool IsDisconnected { get; }

        byte Id { get; }

        string Name { get; }

        void Connect();

        void Disconnect();
    }
}
