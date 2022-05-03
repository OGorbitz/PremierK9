using System.Collections.Generic;
using System.ComponentModel;

namespace Mobile
{
    public interface IValidatable<T>
    {
        List<IValidationRule<T>> Validations { get; }

        List<string> Errors { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        bool Validate();

        bool IsValid { get; set; }
    }
}
