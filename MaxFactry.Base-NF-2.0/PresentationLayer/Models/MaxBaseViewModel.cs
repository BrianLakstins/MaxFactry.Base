// <copyright file="MaxBaseViewModel.cs" company="Lakstins Family, LLC">
// Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// </copyright>

#region License
// <license>
// This software is provided 'as-is', without any express or implied warranty. In no
// event will the author be held liable for any damages arising from the use of this
// software.
//
// Permission is granted to anyone to use this software for any purpose, including
// commercial applications, and to alter it and redistribute it freely, subject to the
// following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that
// you wrote the original software. If you use this software in a product, an
// acknowledgment (see the following) in the product documentation is required.
//
// Portions Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
//
// 2. Altered source versions must be plainly marked as such, and must not be
// misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.
// </license>
#endregion License

#region Change Log
// <changelog>
// <change date="8/3/2020" author="Brian A. Lakstins" description="Initial creation">
// <change date="8/3/2020" author="Brian A. Lakstins" description="Add public methods to relate text to the model.">
// </changelog>
#endregion Change Log

namespace MaxFactry.Base.PresentationLayer
{
    using System;
#if net4_52 || netcore1 || netstandard1_2
    using System.Linq.Expressions;
#endif
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;

    /// <summary>
    /// View model base
    /// </summary>
    public abstract class MaxBaseViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Internal storage of original values
        /// </summary>
        private MaxIndex _oOriginalValues = new MaxIndex();

        /// <summary>
        /// Internal storage of view data
        /// </summary>
        private MaxIndex _oValueIndex = new MaxIndex();

        /// <summary>
        /// Initializes a new instance of the MaxBaseViewModel class.
        /// </summary>
        public MaxBaseViewModel()
        {
        }

        /// <summary>
        /// Gets or sets original values for comparison after updates
        /// </summary>
        public MaxIndex OriginalValues
        {
            get
            {
                return this._oOriginalValues;
            }

            set
            {
                this._oOriginalValues = value;
            }
        }

        /// <summary>
        /// Gets a value using the format for a property
        /// </summary>
        /// <param name="lsPropertyName">Name of the property</param>
        /// <param name="loValue">Value to format</param>
        /// <returns>Formatted text</returns>
        protected virtual string GetFormatted(string lsPropertyName, object loValue)
        {
            if (loValue is double && (double)loValue == double.MinValue)
            {
                return string.Empty;
            }

            string lsR = loValue.ToString(); ;
            string lsFormat = MaxMetaLibrary.GetMetaText(typeof(object), this, lsPropertyName, "MaxMeta.Format");
            if (null != lsFormat && lsFormat.Length > 0)
            {
                lsR = MaxConvertLibrary.Format(typeof(object), loValue, lsFormat);
            }

            return lsR;
        }

        /// <summary>
        /// Sets some text related to a key.
        /// </summary>
        /// <param name="lsKey">The key to use for lookup</param>
        /// <param name="lsValue">The value of the variable</param>
        public virtual void SetText(string lsKey, string lsValue)
        {
            this._oValueIndex.Add(lsKey, lsValue);
        }

        /// <summary>
        /// Gets some text related to the key
        /// </summary>
        /// <param name="lsKey">Key used for lookup</param>
        /// <param name="lsDefault">A default value if the key does not exist.</param>
        /// <returns>Text related to the key.</returns>
        public virtual string GetText(string lsKey, string lsDefault)
        {
            return this._oValueIndex.GetValueString(lsKey, lsDefault);
        }

        /// <summary>
        /// Gets the label text used when a value for the property is requested
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Text to use as the label</returns>
        public virtual string GetLabelText(string lsProperty)
        {
            string lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Label");
            if (null == lsR || lsR.Length == 0)
            {
                lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Name");
            }

            if (null == lsR || lsR.Length == 0)
            {
                lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Display.Name");
            }

            return lsR;
        }

        /// <summary>
        /// Gets the header text for the property when shown in a list
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Text to use as the header</returns>
        public virtual string GetListHeaderText(string lsProperty)
        {
            string lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.ListHeader");

            if (null == lsR || lsR.Length == 0)
            {
                lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Display.ShortName");
            }

            return lsR;
        }

        /// <summary>
        /// Gets the description text for the property
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Text to use as the description</returns>
        public virtual string GetDescriptionText(string lsProperty)
        {
            string lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Description");

            if (null == lsR || lsR.Length == 0)
            {
                lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Display.Description");
            }

            return lsR;
        }

        /// <summary>
        /// Gets the hint text for the property
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Text to use as the hint</returns>
        public virtual string GetHintText(string lsProperty)
        {
            string lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Hint");

            if (null == lsR || lsR.Length == 0)
            {
                lsR = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Display.Prompt");
            }

            return lsR;
        }

        /// <summary>
        /// Gets the maximum value that should be used for the property
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Maximum value (double.MinValue if none is set)</returns>
        public virtual double GetRangeMax(string lsProperty)
        {
            double lnR = double.MinValue;
            string lsValue = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Maximum");

            if (null == lsValue || lsValue.Length == 0)
            {
                lsValue = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Range.Maximum");
            }

            if (null != lsValue && lsValue.Length > 0)
            {
                lnR = MaxConvertLibrary.ConvertToDouble(typeof(object), lsValue);
            }

            return lnR;
        }

        /// <summary>
        /// Gets the minimum value that should be used for the property
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Minimum value (double.MaxValue if none is set)</returns>
        public virtual double GetRangeMin(string lsProperty)
        {
            double lnR = double.MaxValue;
            string lsValue = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Minimum");

            if (null == lsValue || lsValue.Length == 0)
            {
                lsValue = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Range.Minimum");
            }

            if (null != lsValue && lsValue.Length > 0)
            {
                lnR = MaxConvertLibrary.ConvertToDouble(typeof(object), lsValue);
            }

            return lnR;
        }

        /// <summary>
        /// Gets the increment value that should be used when the value is changed
        /// </summary>
        /// <param name="lsProperty">Name of the property</param>
        /// <returns>Increment value (0 if none is set)</returns>
        public virtual double GetIncrement(string lsProperty)
        {
            double lnR = 0;
            string lsValue = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "MaxMeta.Increment");

            if (null == lsValue || lsValue.Length == 0)
            {
                lsValue = MaxMetaLibrary.GetMetaText(typeof(object), this, lsProperty, "Range.Minimum");
            }

            if (null != lsValue && lsValue.Length > 0)
            {
                lnR = MaxConvertLibrary.ConvertToDouble(typeof(object), lsValue);
            }

            return lnR;
        }

        /// <summary>
        /// Gets the list of properties to display
        /// </summary>
        /// <returns>Array of text with property names</returns>
        public virtual string[] GetPropertyDisplayList()
        {
            MaxIndex loIndex = MaxMetaLibrary.GetPropertyDisplayList(typeof(object), this);
            string[] laKey = loIndex.GetSortedKeyList();
            string[] laR = new string[laKey.Length];
            for (int lnR = 0; lnR < laR.Length; lnR++)
            {
                laR[lnR] = MaxConvertLibrary.ConvertToString(typeof(object), loIndex[laKey[lnR]]);
            }

            return laR;
        }

        /// <summary>
        /// Sets a value for the property 
        /// </summary>
        /// <param name="lsPropertyName">Name of the property</param>
        /// <param name="loValue">Value to set</param>
        /// <returns>True if the value is updated.  False if it is the same.</returns>
        protected virtual bool Set(string lsPropertyName, object loValue)
        {
            bool lbUpdate = true;
            string lsKey = MaxMetaLibrary.GetKey(typeof(object), this, lsPropertyName);
            if (this._oValueIndex.Contains(lsKey))
            {
                lbUpdate = false;
                object loCurrent = this._oValueIndex[lsKey];
                if (null != loCurrent || null != loValue)
                {
                    lbUpdate = true;
                    if (null != loCurrent && null != loValue && loCurrent.Equals(loValue))
                    {
                        lbUpdate = false;
                    }
                }
            }

            if (lbUpdate)
            {
                if (!this._oOriginalValues.Contains(lsKey))
                {
                    this._oOriginalValues.Add(lsKey, loValue);
                }

                this._oValueIndex.Add(lsKey, loValue);
                this.OnPropertyChanged(lsPropertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <param name="lsPropertyName">Name of the property</param>
        /// <returns>object value of the property</returns>
        protected virtual object Get(string lsPropertyName)
        {
            object loR = null;
            string lsKey = MaxMetaLibrary.GetKey(typeof(object), this, lsPropertyName);
            bool lbHasKey = this._oValueIndex.Contains(lsKey);
            if (lbHasKey)
            {
                loR = this._oValueIndex[lsKey];
            }

            return loR;
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Event to handle when a property is changed
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// For when a property value is changed
        /// </summary>
        /// <param name="lsPropertyName">Name of the property</param>
        protected virtual void OnPropertyChanged(string lsPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                System.ComponentModel.PropertyChangedEventArgs loArgs = new System.ComponentModel.PropertyChangedEventArgs(lsPropertyName);
                this.PropertyChanged(this, loArgs);
            }
        }

        #endregion

        /// <summary>
        /// Gets a name that can be used in a DataTable column
        /// </summary>
        /// <param name="lsName">Current name</param>
        /// <returns>Name with special characters removed</returns>
        public string GetDataTableColumnName(string lsName)
        {
            return lsName.Replace("(", string.Empty).Replace(")", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace(".", string.Empty).Replace("/", string.Empty).Replace(@"\", string.Empty);
        }

#if net4_52 || netcore1 || netstandard1_2

        /// <summary>
        /// Gets a property name when specified with a function to the property
        /// </summary>
        /// <typeparam name="T">Generic placeholder for the property type</typeparam>
        /// <param name="loFunction">Function that specifies the type</param>
        /// <returns>Name of the property</returns>
        public string GetPropertyName<T>(Expression<Func<T>> loFunction)
        {
            string lsR = string.Empty;
            if (null != loFunction)
            {
                MemberExpression loBody = loFunction.Body as MemberExpression;
                if (null != loBody)
                {
                    lsR = loBody.Member.Name;
                }
            }

            return lsR;
        }

        protected virtual bool Set<T>(Expression<Func<T>> loFunction, T loValue)
        {
            string lsName = this.GetPropertyName(loFunction);
            return this.Set(lsName, loValue);
        }

        protected virtual T Get<T>(Expression<Func<T>> loFunction)
        {
            string lsName = this.GetPropertyName(loFunction);
            object loValue = this.Get(lsName);
            if (null != loValue)
            {
                return (T)loValue;
            }

            return default(T);
        }

#endif
    }
}