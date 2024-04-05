﻿using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.DataContext;
using BrunoTheBot.APIs.Service;
using BrunoTheBot.DataContext.Repositories;
using BrunoTheBot.DataContext.DataService.Repository.Course;

namespace BrunoTheBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillSchoolRepositoryController : ControllerBase
    {
        private readonly AILogRepository _aILogRepository;
        private readonly SchoolRepository _schoolRepository;

        public FillSchoolRepositoryController(AILogRepository aILogRepository, SchoolRepository schoolRepository)
        {
            _aILogRepository = aILogRepository;
            _schoolRepository = schoolRepository;
        }

        [HttpGet("FillSchoolRepository")]
        public async Task<ActionResult<string>> FillSchoolRepositoryFromAILog(int AILogID)
        {
            try
            {
                var content = await _aILogRepository.GetAILogByIdAsync(AILogID);

                if (content == null)
                {
                    return StatusCode(404, $"No content found for the specified ID: {AILogID}");
                }

                // Parse the JSON content using ChatGPTContentJSONParse
                var parsedContent = ChatGPTContentJSONParse.MainParse(content.JSON);
                var newSchool = ChatGPTContentJSONParse.PopulateSchool(parsedContent);

                await _schoolRepository.CreateSchoolAsync(newSchool);

                return Ok(parsedContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error GetSchoolFeatures: {ex.Message}");
            }
        }
    }
}