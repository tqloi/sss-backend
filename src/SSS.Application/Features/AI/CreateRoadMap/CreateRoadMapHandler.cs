using MediatR;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Features.AI.Common;
using SSS.Application.Features.Content.Roadmaps.Common;
using SSS.Application.Features.Content.Roadmaps.GraphCreate;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed class CreateRoadMapHandler(IPipeLine pipeLine,
    IMediator mediator
) : IRequestHandler<CreateRoadMapCommand, CreateRoadMapResult>
    {
        public async Task<CreateRoadMapResult> Handle(
            CreateRoadMapCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Call GPT
            var rawJson = await pipeLine.GenerateRoadmapAsync(
                request.Message,
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

            return new CreateRoadMapResult
            {
                Success = true,
                Message = "AI roadmap generated successfully",
                Rawroadmap = doc.RootElement.Clone()
            };
        }
    }
}
