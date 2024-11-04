using IFC_Table_View.Infracrucrure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace IFC_Table_View.IFC.ModelItem
{



    public abstract class BaseModelItemIFC : INotifyPropertyChanged
    {
        public abstract object ItemTreeView { get; }

        public abstract Dictionary<string, HashSet<object>> PropertyElement { get; }

        public abstract ObservableCollection<BaseModelItemIFC> ModelItems { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Событие изменения элемента
        /// </summary>
        /// <param name = "PropertyName" ></ param >
        protected virtual void OnPropertyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        private bool _IsExpanded { get; set; } = false;
        /// <summary>
        /// IsExpanded
        /// </summary>
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        private bool _IsSelected { get; set; } = false;
        ///  <summary>
        ///  IsSelected 
        ///  </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value)
                {
                    FontWeight = FontWeights.Bold;
                }
                else
                {
                    FontWeight = FontWeights.Normal;
                }

                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }


        private FontWeight _FontWeight { get; set; } = FontWeights.Normal;
        ///  <summary>
        /// IsSelected 
        /// </summary>
        public FontWeight FontWeight
        {
            get { return _FontWeight; }
            set
            {
                _FontWeight = value;
                OnPropertyChanged("FontWeight");
            }
        }

        private bool _IsFocusReference { get; set; } = false;
        /// <summary>
        /// Фокус элемента
        /// </summary>
        public bool IsFocusReference
        {
            get { return _IsFocusReference; }
            set
            {
                _IsFocusReference = value;
                OnPropertyChanged("IsFocusReference");
            }
        }


    }
}