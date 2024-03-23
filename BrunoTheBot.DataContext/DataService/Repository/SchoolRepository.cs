﻿using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities;

namespace BrunoTheBot.DataContext
{
    public class SchoolRepository
    {
        private readonly SqliteDataContext _context;

        public SchoolRepository(SqliteDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateSchoolAsync(School school)
        {
            try
            {
                EnsureSchoolsNotNull();
                _context.Schools!.Add(school);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the school:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<School?> GetSchoolByIdAsync(int id)
        {
            EnsureSchoolsNotNull();
            return await _context.Schools.FindAsync(id);
        }

        public async Task<IQueryable<School>> GetAllSchoolsAsync()
        {
            EnsureSchoolsNotNull();
            return _context.Schools.AsQueryable();
        }

        public async Task UpdateSchoolAsync(School school)
        {
            EnsureSchoolsNotNull();
            _context.Entry(school).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSchoolAsync(int id)
        {
            EnsureSchoolsNotNull();
            var school = await _context.Schools.FindAsync(id);
            if (school != null)
            {
                _context.Schools.Remove(school);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureSchoolsNotNull()
        {
            if (_context.Schools == null)
            {
                throw new InvalidOperationException("The Schools DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}