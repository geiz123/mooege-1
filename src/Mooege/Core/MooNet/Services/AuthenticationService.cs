﻿/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using Mooege.Common;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:bnet.protocol.authentication.AuthenticationServer, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }

        public override void Logon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.LogonRequest request, Action<bnet.protocol.authentication.LogonResponse> done)
        {
            Logger.Trace("LogonRequest(); Email={0}", request.Email);
            var account = AccountManager.GetAccountByEmail(request.Email) ?? AccountManager.CreateAccount(request.Email); // add a config option that sets this functionality, ie AllowAccountCreationOnFirstLogin.

            Client.Account = account;
            Client.Account.LoggedInClient = Client;

            var builder = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(Client.Account.BnetAccountID)
                .SetGameAccount(Client.Account.BnetGameAccountID);

            done(builder.Build());

            PlayerManager.PlayerConnected(this.Client);
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.ModuleMessageRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
