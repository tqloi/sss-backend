using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;

namespace SSS.Application.Features.Surveys.SurveyAnswers.DeleteAnswer
{
    public class DeleteAnswerHandler(IAppDbContext db) 
        : IRequestHandler<DeleteAnswerCommand, DeleteAnswerResponse>
    {
        public async Task<DeleteAnswerResponse> Handle(
            DeleteAnswerCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var answer = await db.SurveyAnswers
                    .Include(a => a.Response)
                    .FirstOrDefaultAsync(x => x.Id == request.AnswerId, cancellationToken);

                if (answer == null)
                    throw new NotFoundException("Answer not found");

                // Ki?m tra response ?ã submit ch?a
                if (answer.Response.SubmittedAt != null)
                    throw new ForbiddenException("Cannot delete answer from submitted response");

                db.SurveyAnswers.Remove(answer);
                await db.SaveChangesAsync(cancellationToken);

                return new DeleteAnswerResponse(true, "Answer deleted successfully");
            }
            catch (NotFoundException ex)
            {
                return new DeleteAnswerResponse(false, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return new DeleteAnswerResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new DeleteAnswerResponse(false, $"Error deleting answer: {ex.Message}");
            }
        }
    }
}