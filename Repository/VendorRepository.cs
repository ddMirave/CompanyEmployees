using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
    {
        public VendorRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
    }
}
