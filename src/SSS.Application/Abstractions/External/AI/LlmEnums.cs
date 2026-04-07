namespace SSS.Application.Abstractions.External.AI
{
    public enum LlmTask
    {
        SurveyAnalysis,
        LearningProfile,
        GenerateRoadmap,
        GenerateStudyPlan,
        SimpleChat
    }

    public enum LlmProvider
    {
        Gemini,
        Gpt
    }
}