using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext.DataService.Repository.Quiz
{
    public class ResponseRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public ResponseRepository(PostgreBrunoTheBotContext context)
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

        private void EnsureResponseNotNull()
        {
            if (_context.Responses == null)
            {
                throw new InvalidOperationException("CreateTestAsync: The Response DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
