using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Assessment;

public class SurveyQuestion
{
    public long Id { get; set; }
    public long SurveyId { get; set; }
    public string QuestionKey { get; set; } = null!;    
    public string Prompt { get; set; } = null!;
    public SurveyQuestionType Type { get; set; } = SurveyQuestionType.FreeText;
    public int? ScaleMin { get; set; }
    public int? ScaleMax { get; set; }
    public int OrderNo { get; set; }
    public bool IsRequired { get; set; }

    // Navigation
    public virtual Survey Survey { get; set; } = null!;
    public virtual ICollection<SurveyQuestionOption> Options { get; set; } = new HashSet<SurveyQuestionOption>();
    public virtual ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
}