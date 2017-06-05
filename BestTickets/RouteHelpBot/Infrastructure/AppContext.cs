﻿using System.Data.Entity;

namespace RouteHelpBot.Infrastructure
{
    public class AppContext:DbContext
    {
        public AppContext(): base("CitiesHandbook"){ }

        public DbSet<City> Cities { get; set; }
    }
}