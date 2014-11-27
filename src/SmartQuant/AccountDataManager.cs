// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
	public partial class AccountDataManager
	{
        private Framework framework;

        public AccountDataSnapshot GetSnapshot(byte providerId, byte route)
        {
            throw new NotImplementedException();
        }

        public AccountDataSnapshot[] GetSnapshots()
        {
            throw new NotImplementedException();
        }

        internal AccountDataManager(Framework framework)
        {
            this.framework = framework;
        }

        internal void Clear()
        {
//            lock (this.dictionary_0)
//                this.dictionary_0.Clear();
        }
	}
}
