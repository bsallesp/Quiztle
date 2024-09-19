using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Quiztle.DataContext.DataService.Repository.Quiz
{
    public class ResponseRepository
    {
        private readonly PostgreQuiztleContext _context;

        public ResponseRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<APIResponse<Response>> GetResponseNotFinalized(Guid testId)
        {
            try
            {
                EnsureResponseNotNull();
                var response = await _context.Responses!
                                  .FirstOrDefaultAsync(r => r.TestId == testId && r.IsFinalized == false);

                if (response == null) return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new Response(),
                    Message = "Responses Not Finalized with testid " + testId + " not found at this time"
                };

                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = response,
                    Message = "Responses Not Finalized with testid " + testId + " found."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = "ERROR - GETTING CATCH LINE " + testId + ex
                };
            }
        }

        public async Task<APIResponse<Response>> GetResponseById(Guid id)
        {
            try
            {
                EnsureResponseNotNull();
                var response = await _context.Responses!.FirstOrDefaultAsync(r => r.Id == id);

                if (response == null) return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = null,
                    Message = "Response with ID " + id + " not found."
                };

                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = response,
                    Message = "Response found."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = null,
                    Message = "ERROR - GETTING RESPONSE BY ID " + id + ex
                };
            }
        }

        public async Task<APIResponse<Response>> UpdateResponse(Guid id, Response updatedResponse)
        {
            Console.WriteLine("Getting into UpdateResponse in ResponseRepository...");
            try
            {
                EnsureResponseNotNull();
                var response = await _context.Responses!.FirstOrDefaultAsync(r => r.Id == id);

                if (response == null)
                {
                    return new APIResponse<Response>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new Response(),
                        Message = "Response with ID " + id + " not found."
                    };
                }

                response.Name = updatedResponse.Name;
                response.Shots = updatedResponse.Shots;
                response.Created = updatedResponse.Created;
                response.TestId = updatedResponse.TestId;
                response.IsFinalized  = updatedResponse.IsFinalized;
                response.Score = updatedResponse.Score;
                response.Percentage = updatedResponse.Percentage;

                _context.Responses!.Update(response);
                await _context.SaveChangesAsync();

                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = response,
                    Message = "Response updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = "ERROR - UPDATING RESPONSE BY ID " + id + ex
                };
            }
        }


        public async Task<bool> CreateResponse(Response response)
        {
            Console.WriteLine("creating response in repository...");
            EnsureResponseNotNull();
            try
            {
                await _context.Responses!.AddAsync(response);
                await _context.SaveChangesAsync(); // Certifique-se de salvar as mudanças
                Console.WriteLine("creating response in repository... - GOOD");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("creating response in repository... - FAILED");
                return false;
            }
        }

        public async Task<APIResponse<List<Response>>> GetFinalizedResponsesByTestId(Guid testId)
        {
            try
            {
                EnsureResponseNotNull();
                var responses = await _context.Responses!
                                     .Where(r => r.TestId == testId && r.IsFinalized == true)
                                     .ToListAsync();

                if (responses == null || !responses.Any())
                {
                    return new APIResponse<List<Response>>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new List<Response>(),
                        Message = "No finalized responses found for test ID " + testId
                    };
                }

                return new APIResponse<List<Response>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = responses,
                    Message = "Finalized responses found for test ID " + testId
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Response>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Response>(),
                    Message = "ERROR - GETTING FINALIZED RESPONSES: " + ex
                };
            }
        }

        private void EnsureResponseNotNull()
        {
            if (_context.Responses == null)
            {
                throw new InvalidOperationException("CreateScratchAsync: The Response DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
