namespace Sindvamb.Service.Inputs
{
    public class CreateInput<TInput>
    {
        public TInput Model { get; set; }

        public CreateInput(TInput input) => Model = input;
    }
}
