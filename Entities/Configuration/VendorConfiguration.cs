using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasData
            (
            new Vendor
            {
                Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                Name = "Nkie",
                MarketId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
            },
            new Vendor
            {
                Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                Name = "МирКниг",
                MarketId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
            },
            new Vendor
            {
                Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                Name = "Ababas",
                MarketId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
            }
            );

        }
    }
}
