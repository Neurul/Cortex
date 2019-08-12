﻿using CQRSlite.Commands;
using Nancy;
using Nancy.Security;
using org.neurul.Cortex.Application.Neurons.Commands;
using org.neurul.Cortex.Port.Adapter.Common;
using System;

namespace org.neurul.Cortex.Port.Adapter.In.Api
{
    public class NeuronModule : NancyModule
    {
        public NeuronModule(ICommandSender commandSender) : base("/{avatarId}/cortex/neurons")
        {
            if (bool.TryParse(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.RequireAuthentication), out bool value) && value)
                this.RequiresAuthentication();

            this.Put("/{neuronId}", async (parameters) =>
            {
                // TODO: make idempotent since HTTP PUT should allow any number of calls or change to POST?
                return await Helper.ProcessCommandResponse(
                        commandSender,
                        this.Request,
                        false,
                        (bodyAsObject, bodyAsDictionary, expectedVersion) =>
                        {
                            var neuronId = Guid.Parse(parameters.neuronId);
                            var avatarId = parameters.avatarId;
                            string tag = bodyAsObject.Tag.ToString();
                            var authorId = Guid.Parse(bodyAsObject.AuthorId.ToString());
                            return new CreateNeuron(avatarId, neuronId, tag, authorId);
                        },
                        "Tag",
                        "AuthorId"
                    );
            }
            );

            this.Patch("/{neuronId}", async (parameters) =>
            {
                return await Helper.ProcessCommandResponse(
                        commandSender,
                        this.Request,
                        (bodyAsObject, bodyAsDictionary, expectedVersion) =>
                        {
                            return new ChangeNeuronTag(
                                parameters.avatarId, 
                                Guid.Parse(parameters.neuronId), 
                                bodyAsObject.Tag.ToString(),
                                Guid.Parse(bodyAsObject.AuthorId.ToString()), 
                                expectedVersion
                                );
                        },
                        "Tag",
                        "AuthorId"
                    );
            }
            );

            this.Delete("/{neuronId}", async (parameters) =>
            {
                return await Helper.ProcessCommandResponse(
                        commandSender,
                        this.Request,
                        (bodyAsObject, bodyAsDictionary, expectedVersion) =>
                        {
                            return new DeactivateNeuron(
                                parameters.avatarId,
                                Guid.Parse(parameters.neuronId),
                                Guid.Parse(bodyAsObject.AuthorId.ToString()),
                                expectedVersion
                                );
                        },
                        "AuthorId"
                    );
            }
            );
        }     
    }
}
