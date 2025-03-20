namespace Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(Type type, int id)
            : base($"{type.Name} not found with id: {id}") { }
    }
}
