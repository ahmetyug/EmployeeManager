namespace Core
{
    public interface IValidatableObject
    {
        /// <summary>
        /// Validates the object. Throws <see cref="ArgumentException"/> if invalid.
        /// </summary>
        void Validate();
    }
}
