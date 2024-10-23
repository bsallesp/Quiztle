using Microsoft.AspNetCore.Mvc;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.DataContext.DataService.Repository;
using System.Text.Json;

namespace Quiztle.API.Controllers.Scratches
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateDraftController : ControllerBase
    {
        private readonly DraftRepository _draftRepository;
        private readonly ScratchRepository _scratchRepository;
        private readonly ILLMChatGPTRequest _chatGPTRequest;

        public UpdateDraftController(
            DraftRepository draftRepository,
            ILLMChatGPTRequest chatGPTRequest
            , ScratchRepository scratchRepository)
        {
            _draftRepository = draftRepository;
            _chatGPTRequest = chatGPTRequest;
            _scratchRepository = scratchRepository;
        }

        [HttpPost("UpdateDraftByDraft")]
        public async Task<IActionResult> UpdateDraftByDraftAsync([FromBody] Guid draftGuid)
        {
            try
            {
                var draft = await _draftRepository.GetDraftByIdAsync(draftGuid);

                if (draft == null || draft.OriginalContent == null) return NotFound();

                var summaryLength = (int)Math.Round(draft.OriginalContent.Length * 0.7);
                var prompt = UpdateDraftPromt.GetPromptString(draft.OriginalContent!,
                    new string[]
                    {
                        "Azure Cloud Concepts", "Azure Architecture and Services", "Azure Management and Governance"
                    });

                    //                new string[]
                    //{
                    //                        "Prerequisites for Azure administrators",
                    //                        "Manage identities and governance in Azure",
                    //                        "Configure and manage virtual networks for Azure administrators",
                    //                        "Implement and manage storage in Azure",
                    //                        "Deploy and manage Azure compute resources",
                    //                        "Monitor and back up Azure resources",
                    //});

                Console.WriteLine(prompt);
                
                var llmResult = await _chatGPTRequest.ExecuteAsync(prompt);

                DraftJson newDraft = JsonSerializer.Deserialize<DraftJson>(llmResult)!;

                draft.MadeByAiContent = newDraft.Draft!.MadeByAiContent;
                draft.Title = newDraft.Draft.Title;
                draft.Tag = newDraft.Draft.Tag;
                foreach (var question in draft.Questions!) question.Tag = draft.Tag;

                await _draftRepository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }

        [HttpPost("UpdateDraftByScratch")]
        public async Task<ActionResult> UpdateDraftByScratchAsync([FromBody] Guid scratchGuid)
        {
            var scratch = await _scratchRepository.GetScratchByIdAsync(scratchGuid);
            if (scratch == null) return NotFound();

            foreach (var draft in scratch.Drafts!)
            {
                await UpdateDraftByDraftAsync(draft.Id);
            }

            return Ok();
        }

        public class Draft
        {
            public string? Title { get; set; }
            public string? MadeByAiContent { get; set; }
            public string? Tag { get; set; }
        }

        public class DraftJson
        {
            public Draft? Draft { get; set; }
        }
    }
}