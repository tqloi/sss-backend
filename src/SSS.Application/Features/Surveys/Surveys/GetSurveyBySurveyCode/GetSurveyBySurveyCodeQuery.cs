using MediatR;

namespace SSS.Application.Features.Surveys.Surveys.GetSurveyBySurveyCode;

public sealed record GetSurveyBySurveyCodeQuery(string SurveyCode) : IRequest<GetSurveyBySurveyCodeResult>;