﻿using CQRSlite.Commands;
using org.neurul.Cortex.Domain.Model.Neurons;
using System;
using System.Collections.Generic;

namespace org.neurul.Cortex.Application.Neurons.Commands
{
    public class AddTerminalsToNeuron : ICommand
    {
        public readonly IEnumerable<Terminal> Terminals;

        public AddTerminalsToNeuron(string avatarId, Guid id, IEnumerable<Terminal> terminals, int originalVersion)
        {
            this.AvatarId = avatarId;
            this.Id = id;            
            this.Terminals = terminals;
            this.ExpectedVersion = originalVersion;
        }

        public string AvatarId { get; private set; }

        public Guid Id { get; private set; }
        
        public int ExpectedVersion { get; set; }
    }
}
