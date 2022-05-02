using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mobile.Helpers.Behaviors
{
public class EntryLineValidationBehavior : BehaviorBase<Entry>
{
    #region StaticFields
    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(EntryLineValidationBehavior), true, BindingMode.Default, null, (bindable, oldValue, newValue) => OnIsValidChanged(bindable, newValue));
    #endregion
    #region Properties
    public bool IsValid
    {
        get
        {
            return (bool)GetValue(IsValidProperty);
        }
        set
        {
            SetValue(IsValidProperty, value);
        }
    }
        #endregion
        #region StaticMethods
        private static async void OnIsValidChanged(BindableObject bindable, object newValue)
        {
            if (bindable is EntryLineValidationBehavior IsValidBehavior &&
                 newValue is bool IsValid)
            {
                IsValidBehavior.AssociatedObject.PlaceholderColor = IsValid ? Color.Default : Color.Red;

                if (!IsValid)
                {
                    uint timeout = 50;
                    await IsValidBehavior.AssociatedObject.TranslateTo(-15, 0, timeout);

                    await IsValidBehavior.AssociatedObject.TranslateTo(15, 0, timeout);

                    await IsValidBehavior.AssociatedObject.TranslateTo(-10, 0, timeout);

                    await IsValidBehavior.AssociatedObject.TranslateTo(10, 0, timeout);

                    await IsValidBehavior.AssociatedObject.TranslateTo(-5, 0, timeout);

                    await IsValidBehavior.AssociatedObject.TranslateTo(5, 0, timeout);

                    IsValidBehavior.AssociatedObject.TranslationX = 0;
                }
            }
        }

        #endregion
    }


    public class BehaviorBase<T> : Behavior<T>
where T : BindableObject
{
    #region Properties
    public T AssociatedObject
    {
        get;
        private set;
    }
    #endregion
    #region NormalMethods
    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        OnBindingContextChanged();
    }
    #endregion
    #region Overrides
    protected override void OnAttachedTo(T bindable)
    {
        base.OnAttachedTo(bindable);
        AssociatedObject = bindable;
        if (bindable.BindingContext != null)
        {
            BindingContext = bindable.BindingContext;
        }

        bindable.BindingContextChanged += OnBindingContextChanged;
    }
    protected override void OnDetachingFrom(T bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.BindingContextChanged -= OnBindingContextChanged;
        AssociatedObject = null;
    }
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        BindingContext = AssociatedObject.BindingContext;
    }
    #endregion
}
}
