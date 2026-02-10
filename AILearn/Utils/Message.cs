using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AILearn.Utils;

public class QuestionAnsweredMessage : ValueChangedMessage<int>
{
    public QuestionAnsweredMessage(int questionNumber) : base(questionNumber)
    { }
}

public class QuestionPickedMessage
{
    public int QuestionIndex { get; set; }
}