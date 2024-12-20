﻿using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;

namespace Quiztle.DataContext.Repositories.Quiz
{
    public class OptionRepository
    {
        private readonly PostgreQuiztleContext _context;

        public OptionRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateOptionAsync(Option option)
        {
            try
            {
                EnsureOptionsNotNull();
                _context.Options!.Add(option);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<APIResponse<List<Option>>> GetOptionByIdAsync(Guid optionId)
        {
            EnsureOptionsNotNull();
            try
            {
                var options = await _context.Options!
                    .Where(s => s.Id == optionId)
                    .ToListAsync();

                if (options == null || options.Count == 0)
                {
                    return new APIResponse<List<Option>>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new List<Option>(),
                        Message = "Options with Response ID " + optionId + " not found."
                    };
                }

                return new APIResponse<List<Option>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = options,
                    Message = "options found successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Option>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Option>(),
                    Message = "ERROR - GETTING Options BY RESPONSE ID: " + ex.Message
                };
            }
        }

        public async Task<IQueryable<Option>> GetAllOptionsAsync()
        {
            EnsureOptionsNotNull();
            var response = await _context.Options!.ToListAsync();
            return response.AsQueryable();
        }

        public async Task UpdateOptionAsync(Option option)
        {
            EnsureOptionsNotNull();
            _context.Entry(option).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOptionAsync(Guid id)
        {
            EnsureOptionsNotNull();
            var option = await _context.Options!.FindAsync(id);
            if (option != null)
            {
                _context.Options.Remove(option);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureOptionsNotNull()
        {
            if (_context.Options == null)
            {
                throw new InvalidOperationException("The Options DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
