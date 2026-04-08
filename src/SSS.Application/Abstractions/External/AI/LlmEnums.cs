namespace SSS.Application.Abstractions.External.AI
{
    public enum LlmTask
    {
        SurveyAnalysis,
        LearningProfile,
        GenerateRoadmap,
        GenerateStudyPlan,
        SimpleChat,
        GenerateQuiz,
        GenerateResultsSummary,
        GenerateBehavioralAnalysis,
        GenerateSurveyAnalysis,
    }

    public enum LlmProvider
    {
        Gemini,
        Gpt
    }
}