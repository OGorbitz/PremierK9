namespace Mobile
{
    public class IsLengthValidRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public int MinimumLength { get; set; }
        public int MaximunLength { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            return (str.Length >= MinimumLength && str.Length <= MaximunLength);
        }
    }
}
