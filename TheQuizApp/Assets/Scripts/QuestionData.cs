using System;

// Custom DataType for storing question Answer, DifficultyRate, and InterestRate
//      and InterestRate in a single list
public class QuestionData : IComparable<QuestionData>
{  
    public string Question { get; set; }
    public string A1 { get; set; }
    public string A2 { get; set; }
    public string A3 { get; set; }
    public string A4 { get; set; }
    public int QuestionNumber { get; set; }
    public int AnswerNumber { get; set; }
	public int DifficultyRate { get; set; }
    public int InterestRate { get; set; }

    public QuestionData(string question, string a1, string a2, string a3, string a4, int questionNumber, int answerNumber, int difficultyRate, int interestRate)
    {
        QuestionNumber = questionNumber;
        Question = question;
        A1 = a1;
        A2 = a2;
        A3 = a3;
        A4 = a4;
        AnswerNumber = answerNumber;
        DifficultyRate = difficultyRate;
        InterestRate = interestRate;
    }

    // Creates a new sorting mechanism by overriding the CompareTo method
    public int CompareTo(QuestionData questionData)
    {
        // Difficulty plays twice more role than the interest rate as 
        //      the users might quit if the questions are too difficult
        int thisCmp = 2 * DifficultyRate + (9 - InterestRate);
        int cmp = 2 * questionData.DifficultyRate + (9 - questionData.InterestRate);

        if (thisCmp < cmp)
        {
            return -1;
        }
        // If the first score is bigger than the second one, swap them, 
        //      to sort the list from small to bigger values
        else if (thisCmp > cmp) 
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
