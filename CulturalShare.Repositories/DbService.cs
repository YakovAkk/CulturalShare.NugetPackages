﻿using Microsoft.EntityFrameworkCore;
using System;

namespace CulturalShare.Repositories;

public class DbService<T> where T : DbContext
{
    protected readonly DbContextOptions<T> dbContextOptions;

    protected DbService(DbContextOptions<T> dbContextOptions)
    {
        this.dbContextOptions = dbContextOptions;
    }

    protected T CreateDbContext()
    {
        return (T)Activator.CreateInstance(typeof(T), dbContextOptions);
    }
}
