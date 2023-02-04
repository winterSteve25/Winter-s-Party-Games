namespace Games.GameModes.WordMania.PromptManagement
{
    public struct WordManiaPromptSubmission
    {
        public readonly int Prompt1;
        public readonly int Prompt2;
        public readonly string FirstAnswer;
        public readonly string SecondAnswer;

        public WordManiaPromptSubmission(int prompt1, int prompt2, string firstAnswer, string secondAnswer)
        {
            Prompt1 = prompt1;
            Prompt2 = prompt2;
            FirstAnswer = firstAnswer;
            SecondAnswer = secondAnswer;
        }

        public override string ToString()
        {
            return $"Prompt 1: {Prompt1}, Prompt 2: {Prompt2}, First: {FirstAnswer}, Second: {SecondAnswer}";
        }
    }
}