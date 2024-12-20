﻿using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Quiztle.DataContext.DataService.Repository.Quiz
{
    public class ShotRepository
    {
        private readonly PostgreQuiztleContext _context;

        public ShotRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<APIResponse<List<Shot>>> GetShotsByResponseId(Guid responseId)
        {
            EnsureShotNotNull();
            try
            {
                var shots = await _context.Shots!
                    .Where(s => s.ResponseId == responseId)
                    .ToListAsync();

                if (shots == null || shots.Count == 0)
                {
                    return new APIResponse<List<Shot>>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new List<Shot>(),
                        Message = "Shots with Response ID " + responseId + " not found."
                    };
                }

                return new APIResponse<List<Shot>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = shots,
                    Message = "Shots found successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Shot>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Shot>(),
                    Message = "ERROR - GETTING SHOTS BY RESPONSE ID: " + ex.Message
                };
            }
        }

        public async Task<APIResponse<Shot>> CreateShot(Shot shot)
        {
            EnsureShotNotNull();
            try
            {
                await _context.Shots!.AddAsync(shot);
                await _context.SaveChangesAsync();

                return new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = shot,
                    Message = "Shot created successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = null,
                    Message = "ERROR - CREATING SHOT: " + ex
                };
            }
        }

        public async Task<APIResponse<bool>> DeleteShot(Guid shotId, Guid responseId)
        {
            EnsureShotNotNull();
            try
            {
                var shot = await _context.Shots!.FirstOrDefaultAsync(s => s.OptionId == shotId && s.ResponseId == responseId);
                if (shot == null)
                {
                    return new APIResponse<bool>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = false,
                        Message = "Shot with ID " + shotId + " not found."
                    };
                }

                _context.Shots!.Remove(shot);
                await _context.SaveChangesAsync();

                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = true,
                    Message = "Shot deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = "ERROR - DELETING SHOT: " + ex
                };
            }
        }

        public async Task<APIResponse<Shot>> GetShotByResponseId(Guid responseId)
        {
            EnsureShotNotNull();
            try
            {
                var shot = await _context.Shots!
                    .FirstOrDefaultAsync(s => s.ResponseId == responseId);

                if (shot == null)
                {
                    return new APIResponse<Shot>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new Shot(),
                        Message = "Shots with Response ID " + responseId + " not found."
                    };
                }

                return new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = shot,
                    Message = "Shots found successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Shot(),
                    Message = "ERROR - GETTING SHOTS BY RESPONSE ID: " + ex
                };
            }
        }

        private void EnsureShotNotNull()
        {
            if (_context.Shots == null)
            {
                throw new InvalidOperationException("The Shot DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
