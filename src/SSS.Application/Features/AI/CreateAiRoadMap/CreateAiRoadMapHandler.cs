using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Text.Json;

namespace SSS.Application.Features.AI.CreateAiRoadMap
{
    public sealed class CreateAiRoadMapHandler(IPipeLine pipeLine,
    IMediator mediator, IAppDbContext dbContext
) : IRequestHandler<CreateAiRoadMapCommand, CreateAiRoadMapResult>
    {
        public async Task<CreateAiRoadMapResult> Handle(
            CreateAiRoadMapCommand request,
            CancellationToken cancellationToken)
        {
            var contentManagerSubject = await dbContext.ContentManagerSubjects.AsNoTracking().FirstOrDefaultAsync(cms => cms.ContentManagerId == request.ManagerId);

            if (contentManagerSubject == null) {
                return new CreateAiRoadMapResult
                {
                    Success = false,
                    Message = $"Content manager with ID {request.ManagerId} not found"
                };
            }
            // 1. Call GPT
            var rawJson = await pipeLine.GenerateRoadmapAsync(
                request.Message,
                contentManagerSubject.SubjectId.ToString(),
                ct: cancellationToken
            );
            Console.Write(rawJson);

            //            if (string.IsNullOrWhiteSpace(rawJson))
            //                throw new ApplicationException("AI returned empty roadmap");

            //            // 2. Deserialize JSON → GraphCreateRequest
            //            RoadmapGraphCreateRequest graphRequest;
            //            try
            //            {
            //                graphRequest = JsonSerializer.Deserialize<RoadmapGraphCreateRequest>(
            //                        rawJson,
            //                        new JsonSerializerOptions
            //                        {
            //                        PropertyNameCaseInsensitive = true,
            //                            Converters =
            //                        {
            //            new JsonStringEnumConverter()
            //        }
            //    }
            //);
            //            }
            //            catch (Exception ex)
            //            {
            //                throw new ApplicationException("Failed to parse AI roadmap JSON", ex);
            //            }

            //            // 3. Map → CreateRoadmapGraphCommand
            //            var createGraphCommand = new CreateRoadmapGraphCommand
            //            {
            //                Roadmap = graphRequest.Roadmap,
            //                Nodes = graphRequest.Nodes,
            //                Contents = graphRequest.Contents,
            //                Edges = graphRequest.Edges
            //            };

            //            // 4. Save DB using existing handler
            //            var graphResult = await mediator.Send(createGraphCommand, cancellationToken);

            //            if (!graphResult.Success || graphResult.Data is null)
            //                throw new ApplicationException(graphResult.Message ?? "Failed to save roadmap");

            //            // 5. Return AI result + DB info
            rawJson = rawJson
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

            using var doc = JsonDocument.Parse(rawJson);

            return new CreateAiRoadMapResult
            {
                Success = true,
                Message = "AI roadmap generated successfully",
                Rawroadmap = doc.RootElement.Clone()
            };
        }
    }
}
