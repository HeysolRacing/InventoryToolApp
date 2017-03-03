﻿using System.Data.Entity;

namespace InventoryTool.Models
{
    public class InventoryToolContext : DbContext
    {
        public InventoryToolContext() : base("name=InventoryToolContext")
        {
        }

        public DbSet<Fleet> Fleets { get; set; }

        public System.Data.Entity.DbSet<InventoryTool.Models.ExchangeRate> ExchangeRates { get; set; }
    }
}
