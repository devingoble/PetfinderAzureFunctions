using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PetfinderAPIWrapper
{
    public interface IAuthStateEntity
    {
        Task<AccessTokenWrapper> Get();
        void Set(AccessTokenWrapper accessToken);
    }

}
