﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetArchTechChallenge.Shared.Infrastructure.DbContexts
{
    public class DbInitializer
    {
        private readonly IConfiguration _configuration;

        public DbInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize()
        {
            using var context = DbContextFactory.CreateDbContext(_configuration);
            context.Database.Migrate();
        }
    }
}
