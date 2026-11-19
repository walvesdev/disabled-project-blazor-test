using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web;

namespace Sindvamb.Web.Shared.Components.Core
{
    /// <summary>
    /// Base component for all the input component types.
    /// </summary>
    public abstract class BaseInputComponent<TValue> : AppComponentBase //, IDisposable
    {
        protected ValidationMessageStore ValidationMessages { get; set; }

        #region Methods

        /// <inheritdoc/>
        protected async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }

        /// <inheritdoc/>
        protected void OnInitialized()
        {

        }


        /// <summary>
        /// Formats the supplied value to it's valid string representation.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <returns>Returns value formatted as string.</returns>
        protected virtual string FormatValueAsString(TValue value)
            => value?.ToString();

        /// <summary>
        /// Prepares the right value to be sent for validation.
        /// </summary>
        /// <remarks>
        /// In some special cases we need to know what is the right value of the underline component.
        /// Like for example for <see cref="Select{TValue}"/> component where we can have value represented as
        /// a single or multiple value, depending on the context where it is used.
        /// </remarks>
        /// <param name="value">Value to prepare for validation.</param>
        /// <returns>Returns the value that is going to be validated.</returns>
        protected virtual object PrepareValueForValidation(TValue value)
            => value;

        /// <summary>
        /// Handler for <c>@onkeydown</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the keyboard down event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnKeyDownHandler(KeyboardEventArgs eventArgs)
        {
            return KeyDown.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Handler for <c>@onkeypress</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the keyboard pressed event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnKeyPressHandler(KeyboardEventArgs eventArgs)
        {
            return KeyPress.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Handler for <c>@onkeyup</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the keyboard up event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnKeyUpHandler(KeyboardEventArgs eventArgs)
        {
            return KeyUp.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Handler for <c>@onblur</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the focus event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnBlurHandler(FocusEventArgs eventArgs)
        {
            return Blur.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Handler for <c>@onfocus</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the focus event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnFocusHandler(FocusEventArgs eventArgs)
        {
            return OnFocus.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Handler for <c>@onfocusin</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the focus event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnFocusInHandler(FocusEventArgs eventArgs)
        {
            return FocusIn.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Handler for <c>@onfocusout</c> event.
        /// </summary>
        /// <param name="eventArgs">Information about the focus event.</param>
        /// <returns>Returns awaitable task</returns>
        protected virtual Task OnFocusOutHandler(FocusEventArgs eventArgs)
        {
            return FocusOut.InvokeAsync(eventArgs);
        }

        #endregion

        #region Properties              
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Parameter]
        public virtual TValue Value { get; set; } = default!;


        /// <summary>
        /// Occurs when a key is pressed down while the control has focus.
        /// </summary>
        [Parameter] public EventCallback<KeyboardEventArgs> KeyDown { get; set; }

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        [Parameter] public EventCallback<KeyboardEventArgs> KeyPress { get; set; }

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        [Parameter] public EventCallback<KeyboardEventArgs> KeyUp { get; set; }

        /// <summary>
        /// The blur event fires when an element has lost focus.
        /// </summary>
        [Parameter] public EventCallback<FocusEventArgs> Blur { get; set; }

        /// <summary>
        /// Occurs when the input box gains or loses focus.
        /// </summary>
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Occurs when the input box gains focus.
        /// </summary>
        [Parameter] public EventCallback<FocusEventArgs> FocusIn { get; set; }

        /// <summary>
        /// Occurs when the input box loses focus.
        /// </summary>
        [Parameter] public EventCallback<FocusEventArgs> FocusOut { get; set; }

        /// <summary>
        /// If defined, indicates that its element can be focused and can participates in sequential keyboard navigation.
        /// </summary>
        [Parameter] public int? TabIndex { get; set; }

        /// <summary>
        /// Used to provide custom validation value on which the validation will be processed with
        /// the <see cref="Validator"/> handler.
        /// </summary>
        /// <remarks>
        /// Should be used carefully as it's only meant for some special cases when input is used
        /// in a wrapper component, like Autocomplete or SelectList.
        /// </remarks>
        [Parameter] public Func<TValue> CustomValidationValue { get; set; }

        #endregion

        //~BaseInputComponent()
        //{
        //    Dispose();
        //}
    }
}
