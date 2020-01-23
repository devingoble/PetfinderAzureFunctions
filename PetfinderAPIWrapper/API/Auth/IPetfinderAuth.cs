using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PetfinderAPIWrapper.API.Auth
{
    public interface IPetfinderAuth
    {
        Task<AccessTokenWrapper> GetAuthTokenAsync(IDurableEntityClient client);
    }
}
