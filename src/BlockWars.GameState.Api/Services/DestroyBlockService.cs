﻿using System;
using System.Threading.Tasks;
using BlockWars.GameState.Models;

namespace BlockWars.GameState.Api.Services
{
    public class DestroyBlockService : IDestroyBlock
    {
        public Task DestroyBlockAsync(string regionId, DestroyRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
