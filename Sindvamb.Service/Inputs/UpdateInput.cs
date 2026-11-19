namespace Sindvamb.Service.Inputs
{
    public class UpdateInput<TInput>
    {
        public TInput Model { get; set; }

        public UpdateInput(TInput input) => Model = input;
    }
}
