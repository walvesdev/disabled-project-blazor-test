namespace Sindvamb.Service.Inputs
{
    public class DeleteInput<TInput, TType>
    {
        public TInput Id { get; set; }

        public DeleteInput(TInput input) => Id = input;
    }
}
