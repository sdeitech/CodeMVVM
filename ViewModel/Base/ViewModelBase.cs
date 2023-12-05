using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ViewModel.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            IntiliazedViewModel();
            IntitializeCollections();

        }

        /// <summary>
        /// Performs intial loading of any view model specific objects
        /// </summary>
        protected virtual void IntiliazedViewModel()
        {

        }

        /// <summary>
        /// Used to initilize and/or load any collections from within the constructor
        /// </summary>
        public abstract void IntitializeCollections();

        #region Fields

        private Dictionary<string, object> _values = new Dictionary<string, object>();

        #endregion Fields

        #region Protected

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);
            SetValue(propertyName, value);
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid_PropertyName", propertyName);
            }

            if (_values == null)
            {
                _values = new Dictionary<string, object>();
            }
            if (!(_values.ContainsKey(propertyName) && _values[propertyName] != null && _values[propertyName].Equals(value)))
            {
                _values[propertyName] = value;
                NotifyPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);
            return GetValue<T>(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName", "Invalid_PropertyName");
            }

            object value;

            if (_values == null)
            {
                _values = new Dictionary<string, object>();
            }

            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        /// <summary>
        /// Validates current instance properties using Data Annotations.
        /// </summary>
        /// <param name="propertyName">This instance property to validate.</param>
        /// <returns>Relevant error string on validation failure or <see cref="System.String.Empty"/> on validation success.</returns>
        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName", "Invalid_PropertyName");

            string error = string.Empty;
            object value = GetValue(propertyName);

            var results = new List<ValidationResult>(2);

            bool result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results);

            if (!result &&
                (value == null || ((value is int || value is long) && (int)value == 0) ||
                 (value is decimal && (decimal)value == 0)))
                return null;

            if (!result)
            {
                ValidationResult validationResult = results.First();
                error = validationResult.ErrorMessage;
            }

            return error;
        }

        #endregion Protected

        #region Privates


        private object GetValue(string propertyName)
        {
            object value = null;
            if (_values != null && (!_values.TryGetValue(propertyName, out value)))
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);

                if (propertyDescriptor == null)
                    throw new ArgumentNullException("propertyName", "Invalid_PropertyName");

                value = propertyDescriptor.GetValue(this);

                if (value != null)
                    _values.Add(propertyName, value);
            }

            return value;
        }

        #endregion Privates

        #region Change Notification

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler == null)
                return;

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;

            if (propertyChanged == null)
                return;

            string propertyName = GetPropertyName(propertySelector);
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Change Notification

        #region Private

        protected static string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        #endregion

        #region Debugging

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;

            string msg = "Invalid property name: " + propertyName;
            throw new ArgumentException(msg);
        }

        #endregion Debugging

        #region OverridenMethods
        public override string ToString()
        {
            PropertyInfo[] pis = this.GetType().GetProperties();
            string val = "\n\t";
            foreach (PropertyInfo item in pis)
            {
                if (item.CanWrite) val += string.Format("{0}: {1}\n\t", item.Name, item.GetValue(this));
            }
            return val;
        }
        #endregion
    }
}
